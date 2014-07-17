using System;
using System.Globalization;
using System.Xml;
using Business.Services;
using Core.DB;
using Core.Domain;
using Core.Enum;
using Core.Instructure;
using Core.Scheduler.Task.Interface;
using System.Data.SqlTypes;
using System.Net;
using Core.Configuration.Helpers;
using System.Text;

namespace Business.Schedule
{
    public class EmulateUpdateStatusOrder : ITask
    {
        private static DriverStatus _orderStatus = DriverStatus.free;

        /// <summary>
        /// Executes a task
        /// </соответствие типов c# и MS SQLsummary>
        /// <param name="node">Xml node that represents a task description</param>
        public void Execute(XmlNode node)
        {
            String status = String.Empty;
            using (var context = new NavgatorTaxiObjectContext())
            {
                using (var orderService = new OrderService(new EfRepository<Order>(context)))
                {
                    var order = orderService.GetConfirmationOrder();
                    if (order != null)
                    {
                        SetOrderStatus(_orderStatus);
                        status = _orderStatus.ToString();

                        using (WebClient client = new WebClient())
                        {
                            String url = ConfigurationHelper.UrlEmulateUpdateOrderStatus
                                         + "?orderId=" + order.OrderId
                                         + "&status=" + status;
                            Uri uri = new Uri(url);
                            client.Encoding = Encoding.UTF8;
                            try
                            {
                                String res = client.DownloadString(uri);
                            }
                            catch (WebException)
                            {

                            }
                        }
                    }
                    else
                    {
                        _orderStatus = DriverStatus.free;
                    }
                }
            }
        }

        private void SetOrderStatus(DriverStatus status)
        {
            switch (status)
            {
               case DriverStatus.free:
                    _orderStatus = DriverStatus.driving;
                    break;
                case DriverStatus.driving:
                    _orderStatus = DriverStatus.waiting;
                    break;
                case DriverStatus.waiting:
                    _orderStatus = DriverStatus.transporting;
                    break;
                case DriverStatus.transporting:
                    _orderStatus = DriverStatus.complete;
                    break;
                default:
                    _orderStatus = DriverStatus.free;
                    break;
            }

        }
    }
}

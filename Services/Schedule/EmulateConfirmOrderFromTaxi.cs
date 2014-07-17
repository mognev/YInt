using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using Business.Services;
using Core.Configuration.Helpers;
using Core.DB;
using Core.Domain;
using Core.Extension.Entity;
using Core.Extension.XmlConverter;
using Core.Instructure;
using Core.Scheduler.Task.Interface;

namespace Business.Schedule
{
    public class EmulateConfirmOrderFromTaxi : ITask
    {
        private static Boolean _sendConfirm = false;

        /// <summary>
        /// Executes a task
        /// </summary>
        /// <param name="node">Xml node that represents a task description</param>
        public void Execute(XmlNode node)
        {
            if (_sendConfirm)
                return;

            using (var context = new NavgatorTaxiObjectContext())
            {
                using (var orderService = new OrderService(new EfRepository<Order>(context)))
                {
                    var order = orderService.GetOrderForConfirmation();
                    if (order != null && order.OrderDrivers.FirstOrDefault() != null)
                    {
                        using (WebClient client = new WebClient())
                        {
                            String driverId = String.Empty;
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            List<Car> cars = serializer.Deserialize<List<Car>>(order.OrderDrivers.LastOrDefault().Drivers);
                            if (cars != null && cars[0] != null)
                            {
                                driverId = cars[0].Uuid;
                            }

                            String url = ConfigurationHelper.UrlEmulateConfirmOrderFromTaxi
                                         + "?orderId=" + order.OrderId
                                         + "&driverId=" + driverId;
                            Uri uri = new Uri(url);
                            client.Encoding = Encoding.UTF8;
                            try
                            {
                                String res = client.DownloadString(uri);
                                _sendConfirm = true;
                            }
                            catch (WebException e)
                            { 

                            }
                        }
                    }
                }
            }
        }
    }
}

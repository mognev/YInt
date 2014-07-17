
using System.Web.Script.Serialization;
using System.Xml.XPath;
using Core.Extension.Converter;
using Core.Extension.Entity;

namespace Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using Core.Db.Interfaces;
    using Core.Domain;
    using Core.Enum;
    using Services.Interfaces;

    public class SetCarService : ISetCarService
    {
        private readonly IDriverService _driverService;
        private readonly IOrderService _orderService;

        public SetCarService(IDriverService driverService, IOrderService orderService)
        {
            _driverService = driverService;
            _orderService = orderService;
        }


        /// <summary>
        /// Parse xml from Yandex server
        /// </summary>
        /// <param name="xml">String</param>
        /// <returns>HttpStatusCode</returns>
        public HttpStatusCode PasrsePostResponse(String xml)
        {
            HttpStatusCode httpStatus = GetHttpStatusCode(xml);

            if (httpStatus == HttpStatusCode.OK)
            {
                ProcessingOrder(xml);
            }

            return httpStatus;
        }

        private HttpStatusCode GetHttpStatusCode(String xml)
        {
            String driversId = String.Empty;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode orderNode = doc.SelectSingleNode("Request/Orderid");
            if (orderNode != null)
            {
                String order = orderNode.InnerText.Trim();
                if (String.IsNullOrEmpty(order))
                {
                    return HttpStatusCode.BadRequest;
                }

                XmlNode car = doc.SelectSingleNode("Request/Cars/Car");
                if (car == null)
                {
                    return HttpStatusCode.BadRequest;
                }

                XmlNode uuid = car.SelectSingleNode("Uuid");
                if (!String.IsNullOrEmpty(uuid.InnerText.Trim()))
                {
                    driversId = uuid.InnerText.Trim();
                }

                FindDriverInStorage status = this.GetRequestStatusDriver(driversId);

                switch (status)
                {
                    case FindDriverInStorage.CurrentDriverBusy:
                        return HttpStatusCode.Gone;
                    case FindDriverInStorage.NotFoundDriver:
                        return HttpStatusCode.NotFound;
                }



                 return HttpStatusCode.OK;
            }

            return HttpStatusCode.BadRequest;
        }

        private FindDriverInStorage GetRequestStatusDriver(String driverId)
        {
            Int32 id;
            if (Int32.TryParse(driverId, out id)) {
                Driver driver = _driverService.GetDriverById(id);
                if (driver == null)
                {
                    return FindDriverInStorage.NotFoundDriver;
                }

                //if (driver.StatusDriver.HasValue && driver.StatusDriver.Value != 1)
                //{
                //    return FindDriverInStorage.CurrentDriverBusy;
                //}
            }

            return FindDriverInStorage.Success;
        }

        private void ProcessingOrder(String xml)
        {
            Order order;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode orderNode = doc.SelectSingleNode("Request/Orderid");
            if (orderNode != null)
            {
                String orderId = orderNode.InnerText.Trim();

                order = _orderService.GetOrderById(orderId);
                if (order == null)
                {
                    return;
                }

                XmlNode car = doc.SelectSingleNode("Request/Cars/Car");
                List<Car> jsonCarsList = new List<Car>();

                try
                {
                    XmlNode uuid = car.SelectSingleNode("Uuid");
                    XmlNode mapHref = car.SelectSingleNode("MapHref");
                    Int32 driverId;
                    if (Int32.TryParse(uuid.InnerText.Trim(), out driverId))
                    {
                        order.driver_id = driverId;
                    }
                    jsonCarsList.Add(new Car
                    {
                        Uuid = uuid.InnerText.Trim(),
                        //MapHref = mapHref.InnerText.Trim()
                    });
                    
                }
                catch (XPathException) { }

                order.Drivers = new JavaScriptSerializer().Serialize(jsonCarsList);

                List<String> listPhone = new List<String>();
                XmlNodeList phones = doc.SelectNodes("Request/ContactInfo/Phones/Phone");
                foreach (XmlNode phone in phones)
                {
                    listPhone.Add(phone.InnerText.Trim());
                }

                order.Phone = String.Join(";", listPhone);

                XmlNode contactInfoName = doc.SelectSingleNode("Request/ContactInfo/Name");
                if (contactInfoName != null)
                {
                    order.ClientName = contactInfoName.InnerText.Trim();
                }

                XmlNode comments = doc.SelectSingleNode("Request/Comments");
                if (comments != null)
                {
                    order.Comments = comments.InnerText.Trim();
                }

                var addressSource = Converter.ToAddressFromXmlNode(doc.SelectSingleNode("Request/Source"));
                if (addressSource != null)
                {
                    order.FullName = addressSource.FullName;
                    order.ShortName = addressSource.ShortName;
                    order.Lon = addressSource.Lon;
                    order.Lat = addressSource.Lat;
                    order.LocalityName = addressSource.LocalityName;
                    order.StreetName = addressSource.StreetName;
                    order.PremiseNumber = addressSource.PremiseNumber;
                    order.PorchNumber = addressSource.PorchNumber;
                }

                XmlNodeList destinations = doc.SelectNodes("Request/Destinations/Destination");
                if (destinations != null && destinations[0] != null)
                {
                    var addressDestination = Converter.ToAddressFromXmlNode(destinations[0]);
                    if (addressDestination != null)
                    {
                        order.D_FullName = addressDestination.FullName;
                        order.D_ShortName = addressDestination.ShortName;
                        order.D_Lon = addressDestination.Lon;
                        order.D_Lat = addressDestination.Lat;
                        order.D_LocalityName = addressDestination.LocalityName;
                        order.D_StreetName = addressDestination.StreetName;
                        order.D_PremiseNumber = addressDestination.PremiseNumber;
                        order.D_PorchNumber = addressDestination.PorchNumber;
                    }
                }

                XmlNodeList requirements = doc.SelectNodes("Request/Requirements/Require");
                if (requirements != null && requirements.Count > 0)
                {
                    Converter.ToRequirements(requirements, order);
                }

                XmlNode bookingTime = doc.SelectSingleNode("Request/BookingTime");
                DateTime _bookingTime;
                if (DateTime.TryParse(bookingTime.InnerText.Trim(), out _bookingTime))
                {
                    order.BookingTime = _bookingTime;
                }
                order.BookingTimeType = bookingTime.Attributes["type"].Value.Trim();
                order.IsDeleted = false;
                order.YandexAcceptOrder = true;

                _orderService.Update(order);

            }
        }
    }
}

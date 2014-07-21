namespace Business.Services
{
    using Core.Configuration.Helpers;
    using Core.Db.Interfaces;
    using Core.Domain;
    using Core.Enum;
    using Core.Extension.Converter;
    using Core.Extension.Entity;
    using Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web.Script.Serialization;
    using System.Xml;
    using System.Xml.XPath;

    public class RequestCarService : IRequestCarService, IDisposable
    {
        private readonly IDriverService _driverService;
        private readonly IOrderService _orderService;

        public RequestCarService(IDriverService driverService, IOrderService orderService)
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

            /* если передаем яндексу положительный ответ, то
               запускаем внутренню логику обработки заказа */
            if (httpStatus == HttpStatusCode.NonAuthoritativeInformation
                || httpStatus == HttpStatusCode.OK)
            {
                ProcessingOrder(xml);
            }
            return httpStatus;
        }


        /// <summary>
        /// Cancel order
        /// </summary>
        /// <param name="orderId">String OrderId</param>
        /// <param name="reason">String reason</param>
        /// <returns></returns>
        public HttpStatusCode CancelOrder(String orderId, String reason)
        {
            Order order = _orderService.GetOrderByIdForCancel(orderId);
            if (order == null)
            {
                return HttpStatusCode.NotFound;
            }

            if (order.OrderStatus == DriverStatus.waiting.ToString())
            {
                return HttpStatusCode.BadRequest;
            }

            // soft delete order from base
            _orderService.Delete(orderId, reason);
            return HttpStatusCode.OK;
        }

        /// <summary>
        /// Confirm order from Taxi Service
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="driverId"></param>
        /// <returns></returns>
        public HttpStatusCode ComfirmOrderFromTaxiService(String orderId, String driverId)
        {
            Order order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return HttpStatusCode.NotFound;
            }

            ConfirmOrderOnYandex(orderId, driverId);

            return HttpStatusCode.OK;
        }


        /// <summary>
        /// Update driver status in Yandex service
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        public HttpStatusCode UpdateOrderStatus(String orderId, String status, String newcar = null, String extra = null)
        {
            String url = ConfigurationHelper.UrlUpdateStatusOrder
                + "?" + String.Join("=", "clid", ConfigurationHelper.Clid)
                + "&" + String.Join("=", "apikey", ConfigurationHelper.ApiKey)
                + "&" + String.Join("=", "orderid", orderId)
                + "&" + String.Join("=", "status", status);

            if (!String.IsNullOrEmpty(newcar))
            {
                url += "&" + String.Join("=", "newcar", newcar);
            }

            if (!String.IsNullOrEmpty(extra))
            {
                url += "&" + String.Join("=", "extra", extra);
            }

            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return HttpStatusCode.NotFound;
            }

            // update Location for driver if status set is waiting
            // чтобы не появлялось сообщение о неверном выставленном статусе
            if (status == DriverStatus.waiting.ToString("f"))
            {
                if (order.driver_id.HasValue)
                {
                    var driver = _driverService.GetDriverById(order.driver_id.Value);
                    if (driver != null)
                    {
                        using (WebClient client = new WebClient())
                        {
                            var culture = CultureInfo.CreateSpecificCulture("en-US");
                            DateTime date = DateTime.UtcNow;
                            date = TimeZoneInfo.ConvertTimeToUtc(driver.LastSession.Value, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));

                            String urlTrek = ConfigurationHelper.UrlUpdateDriverTrek
                                + "?" + String.Join("=", "clid", ConfigurationHelper.Clid)
                                + "&" + String.Join("=", "latitude", (driver.PosY.HasValue) ? driver.PosY.Value.ToString(culture) : String.Empty)
                                + "&" + String.Join("=", "longitude", (driver.PosX.HasValue) ? driver.PosX.Value.ToString(culture) : String.Empty)
                                + "&" + String.Join("=", "uuid", driver.ID_DRIVER.ToString())
                                + "&" + String.Join("=", "avg_speed", (driver.CurrentSpeed.HasValue) ? driver.CurrentSpeed.Value.ToString() : String.Empty)
                                + "&" + String.Join("=", "direction", "0")
                                + "&" + String.Join("=", "time", (driver.CurrentSpeed.HasValue) ? date.ToString("ddMMyyy:HHmmss") : String.Empty);

                            Uri uri = new Uri(urlTrek);
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
                }
            }

            using (WebClient client = new WebClient())
            {
                Uri uri = new Uri(url);
                client.Encoding = Encoding.UTF8;
                try
                {
                    String res = client.DownloadString(uri);
                }
                catch (WebException)
                {
                    return HttpStatusCode.BadRequest;
                }
            }

            order.OrderStatus = status;
            _orderService.Update(order);

            return HttpStatusCode.OK;
        }

        public void Dispose()
        {
            _driverService.Dispose();
            _orderService.Dispose();
        }

        private HttpStatusCode GetHttpStatusCode(String xml)
        {
            if (String.IsNullOrEmpty(xml))
            {
                return HttpStatusCode.BadRequest;
            }

            List<String> driversId = new List<String>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode orderNode = doc.SelectSingleNode("Request/Orderid");
            if (orderNode != null)
            {
                String orderId = orderNode.InnerText.Trim();
                if (String.IsNullOrEmpty(orderId))
                {
                    return HttpStatusCode.BadRequest;
                }

                XmlNodeList cars = doc.SelectNodes("Request/Cars/Car");
                if (cars != null)
                {
                    foreach (XmlNode car in cars)
                    {
                        try
                        {
                            XmlNode uuid = car.SelectSingleNode("Uuid");
                            if (!String.IsNullOrEmpty(uuid.InnerText.Trim()))
                            {
                                driversId.Add(uuid.InnerText.Trim());
                            }
                        }
                        catch (XPathException) { }
                    }

                    FindDriverInStorage status = this.GetRequestStatusDriver(driversId);

                    switch (status)
                    {
                        case FindDriverInStorage.NotAllFoundDriver:
                            return HttpStatusCode.NonAuthoritativeInformation;
                        case FindDriverInStorage.NotFoundDriver:
                            return HttpStatusCode.NotFound;
                    }
                }

                return HttpStatusCode.OK;
            }

            return HttpStatusCode.BadRequest;
        }

        private FindDriverInStorage GetRequestStatusDriver(List<String> driversId)
        {
            List<Driver> listDriver = _driverService.GetDrivers();

            if (driversId.All(x => listDriver.Select(z => z.ID_DRIVER.ToString()).Contains(x)))
            {
                return FindDriverInStorage.Success;
            }

            if (listDriver.Any(x => driversId.Contains(x.ID_DRIVER.ToString())))
            {
                return FindDriverInStorage.NotAllFoundDriver;
            }

            return FindDriverInStorage.NotFoundDriver;
        }

        private void ProcessingOrder(String xml)
        {
            Order order;
            Boolean updateOrder = false;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode orderNode = doc.SelectSingleNode("Request/Orderid");
            if (orderNode != null)
            {
                String orderId = orderNode.InnerText.Trim();
                order = _orderService.GetOrderById(orderId);
                if (order == null)
                {
                    order = new Order();
                    order.OrderStatus = "new";
                }
                else
                {
                    updateOrder = true;
                }

                XmlNodeList cars = doc.SelectNodes("Request/Cars/Car");

                order.OrderId = orderId;
                List<Car> jsonCarsList = new List<Car>();

                if (cars != null && cars.Count > 0)
                {
                    foreach (XmlNode car in cars)
                    {
                        try
                        {
                            XmlNode uuid = car.SelectSingleNode("Uuid");
                            XmlNode dist = car.SelectSingleNode("Dist");
                            XmlNode time = car.SelectSingleNode("Time");
                            XmlNode mapHref = car.SelectSingleNode("MapHref");
                            jsonCarsList.Add(new Car
                            {
                                Uuid = uuid.InnerText.Trim(),
                                Dist = dist.InnerText.Trim(),
                                Time = time.InnerText.Trim()
                                //MapHref = mapHref.InnerText.Trim()
                            });
                        }
                        catch (XPathException) { }
                    }
                }
                else
                {
                    order.PreOrder = true;
                }

                XmlNode recipient = doc.SelectSingleNode("Request/Recipient");
                order.Loyal = false;
                if (recipient != null && recipient.Attributes["loyal"].Value == "yes")
                {
                    order.Loyal = true;
                }
                order.Blacklisted = false;
                if (recipient != null && recipient.Attributes["blacklisted"].Value == "yes")
                {
                    order.Blacklisted = true;
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
                if (bookingTime != null)
                {
                    if (DateTime.TryParse(bookingTime.InnerText.Trim(), out _bookingTime))
                    {
                        order.BookingTime = _bookingTime;
                    }
                    order.BookingTimeType = bookingTime.Attributes["type"].Value.Trim();
                }

                order.IsDeleted = false;
                order.YandexAcceptOrder = false;

                if (order.OrderDrivers == null)
                {
                    order.OrderDrivers = new List<OrderDrivers>();
                }

                if (cars != null)
                {
                    order.OrderDrivers.Add(new OrderDrivers()
                    {
                        Drivers = new JavaScriptSerializer().Serialize(jsonCarsList),
                        Dt = DateTime.Now
                    });
                }

                if (updateOrder)
                {
                    _orderService.Update(order);
                }
                else
                {
                    _orderService.Insert(order);
                }

            }
        }

        private void ConfirmOrderOnYandex(String orderId, String driverId)
        {

            String url = ConfigurationHelper.UrlConfirmationOrder
                    + "?" + String.Join("=", "clid", ConfigurationHelper.Clid)
                    + "&" + String.Join("=", "apikey", ConfigurationHelper.ApiKey)
                    + "&" + String.Join("=", "uuid", driverId)
                    + "&" + String.Join("=", "orderid", orderId);

            using (WebClient client = new WebClient())
            {
                Uri uri = new Uri(url);
                client.Encoding = Encoding.UTF8;
                try
                {
                    String res = client.DownloadString(uri);
                }
                catch (WebException e)
                {
                    String reason = String.Empty;
                    HttpWebResponse response = (HttpWebResponse)e.Response;

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            reason = "notfound";
                            break;
                        case HttpStatusCode.Gone:
                            reason = "assigned";
                            break;
                        default:
                            break;
                    }

                    CancelOrder(orderId, reason);
                }
            }
        }
    
    }
}

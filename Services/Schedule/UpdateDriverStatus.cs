using System;
using System.Net;
using System.Text;
using System.Xml;
using Business.Services;
using Core.Configuration.Helpers;
using Core.DB;
using Core.Domain;
using Core.Extension.XmlConverter;
using Core.Instructure;
using Core.Scheduler.Task.Interface;

namespace Business.Schedule
{
    public class UpdateDriverStatus : ITask
    {
        /// <summary>
        /// Executes a task
        /// </summary>
        /// <param name="node">Xml node that represents a task description</param>
        public void Execute(XmlNode node)
        {
            using (var context = new NavgatorTaxiObjectContext())
            {
                using (var driverService = new DriverService(new EfRepository<Driver>(context)))
                {

                    String url = ConfigurationHelper.UrlUpdateDriverStatus + "?clid="
                                 + ConfigurationHelper.Clid + "&apikey=" + ConfigurationHelper.ApiKey;

                    String request = driverService.SheduleGetDrivers().ToXmlDriverStatusString();

                    using (WebClient client = new WebClient())
                    {
                        Uri uri = new Uri(url);
                        client.Encoding = Encoding.UTF8;
                        try
                        {
                            String res = client.UploadString(uri, "POST", request);
                        }
                        catch (WebException)
                        {

                        }
                    }
                }
            }
        }
    }
}

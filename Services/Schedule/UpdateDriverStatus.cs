using System;
using System.Diagnostics;
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
using System.IO;

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
                    String request = String.Empty;
                    try
                    {

                        String url = ConfigurationHelper.UrlUpdateDriverStatus + "?clid="
                                     + ConfigurationHelper.Clid + "&apikey=" + ConfigurationHelper.ApiKey;

                        request = driverService.SheduleGetDrivers().ToXmlDriverStatusString();

                        System.Net.ServicePointManager.Expect100Continue = false;
                        Uri uri = new Uri(url);
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                        req.Method = "POST";
                        using (Stream reqs = req.GetRequestStream())
                        {
                            byte[] bytes = Encoding.Default.GetBytes(request);
                            reqs.Write(bytes, 0, bytes.Length);
                        }

                        HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                        if (res.StatusCode == HttpStatusCode.OK)
                        {
                            Trace.TraceInformation("{0} Update driver status success", DateTime.Now.ToString());
                        }
                        else
                        {
                            Trace.TraceInformation("{0} Update driver status error request={1}", DateTime.Now.ToString(), request);
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.TraceInformation("{0} Update driver status error {1} request={2}", DateTime.Now.ToString(), e.Message, request);
                    }

                    Trace.Flush();

                }
            }
        }
    }
}

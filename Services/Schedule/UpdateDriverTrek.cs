﻿using System;
using System.IO;
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
using System.Diagnostics;

namespace Business.Schedule
{
    public class UpdateDriverTrek : ITask
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
                    HttpWebResponse res = null;
                    try
                    {
                        String url = ConfigurationHelper.UrlUpdateDriverTrek;

                        request = "compressed=0&data=" + driverService.SheduleGetDrivers().ToXmlDriverTrekString();

                        System.Net.ServicePointManager.Expect100Continue = false;
                        Uri uri = new Uri(url);
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                        req.Method = "POST";
                        using (Stream reqs = req.GetRequestStream())
                        {
                            byte[] bytes = Encoding.Default.GetBytes(request);
                            reqs.Write(bytes, 0, bytes.Length);
                        }


                        res = (HttpWebResponse)req.GetResponse();
                        if (res.StatusCode == HttpStatusCode.OK)
                        {
                            Trace.TraceInformation("{0} Update driver trek success", DateTime.Now.ToString());
                        }
                        else
                        {
                            Trace.TraceInformation("{0} Update driver trek error request={1}", DateTime.Now.ToString(), request);
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.TraceInformation("{0} Update driver trek error {1} request={2}", DateTime.Now.ToString(), e.Message, request);
                    }
                    finally
                    {
                        if (res != null)
                        {
                            res.Close();
                        }
                    }

                    Trace.Flush();
                }
            }
        }
    }
}

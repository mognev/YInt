using System;
using System.Web.Http.Tracing;
using System.Web.Mvc;
using Core.Configuration.Helpers;

namespace YandexTaxiREST.Infrastructure.Trace
{
    public class RequestorTrace : ITraceWriter
    {

        public void Trace(System.Net.Http.HttpRequestMessage request, string category, TraceLevel level, System.Action<TraceRecord> traceAction)
        {
            if (category == ConfigurationHelper.DebugInformation)
            {
                TraceRecord rec = new TraceRecord(request, category, level);
                traceAction(rec);
                WriteTrace(rec, category);
            }
        }

        protected void WriteTrace(TraceRecord rec, String category)
        {
            var message = String.Format("{2}: {0} -> {1}", DateTime.Now.ToString(), rec.Message, category);
            System.Diagnostics.Trace.WriteLine(message);
            System.Diagnostics.Trace.Flush(); 
        }
    }
}

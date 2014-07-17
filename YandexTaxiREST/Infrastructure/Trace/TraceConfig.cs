using System;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace YandexTaxiREST.Infrastructure.Trace
{
    public class TraceConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            //SystemDiagnosticsTraceWriter traceWriter = 
            //    new SystemDiagnosticsTraceWriter()
            //        {
            //            MinimumLevel = TraceLevel.Info,
            //            IsVerbose = false
            //        };

            configuration.Services.Replace(typeof(ITraceWriter), new RequestorTrace());

        }
    }
}

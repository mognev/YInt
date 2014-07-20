namespace YandexTaxiREST.Infrastructure.Trace
{
    using Core.Configuration.Helpers;
    using System.Diagnostics;

    public class RequestFilter : TraceFilter
    {
        public override bool ShouldTrace(
         TraceEventCache cache,
         string source,
         TraceEventType eventType,
         int id,
         string formatOrMessage,
         object[] args,
         object data1,
         object[] data)
        {
            return formatOrMessage.Contains(ConfigurationHelper.DebugInformation);
        }
    }
}
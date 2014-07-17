using System.Web.Http;
using System.Web.Http.Tracing;

namespace YandexTaxiREST.Infrastructure.Base
{
    abstract public class WebAPIBaseController : ApiController
    {
        protected ITraceWriter writer
        {
            get
            {
                return Configuration.Services.GetTraceWriter();
            }
        }
            

    }
}

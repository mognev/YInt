using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Http.Tracing;
using Business.Services.Interfaces;
using Core.Configuration.Helpers;
using YandexTaxiREST.Infrastructure.Base;

namespace YandexTaxiREST.WebAPI
{
    public class RequestCarController : WebAPIBaseController
    {
        private readonly IRequestCarService _requestCarService;
        private readonly IOrderService _orderService;

        public RequestCarController(IRequestCarService requestCarService, IOrderService orderService)
        {
            _requestCarService = requestCarService;
            _orderService = orderService;
        }

        // POST api/values
        public HttpResponseMessage Post()
        {
            HttpRequest _request = HttpContext.Current.Request;
            StreamReader stream = new StreamReader(_request.InputStream);
            string xml = HttpUtility.UrlDecode(stream.ReadToEnd());

            if (writer != null)
            {
                writer.Info(Request, ConfigurationHelper.DebugInformation, "RequestCar -> " + xml);
            }

            return new HttpResponseMessage(_requestCarService.PasrsePostResponse(xml));
        }
    }
}
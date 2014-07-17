using System;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Http.Tracing;
using Business.Services.Interfaces;
using Core.Configuration.Helpers;
using YandexTaxiREST.Infrastructure.Base;

namespace YandexTaxiREST.WebAPI
{
    public class SetCarController : WebAPIBaseController
    {
        private readonly ISetCarService _setCarService;

        public SetCarController(ISetCarService setCarService)
        {
            _setCarService = setCarService;
        }

        // POST api/values
        public HttpResponseMessage Post()
        {
            HttpRequest _request = HttpContext.Current.Request;
            StreamReader stream = new StreamReader(_request.InputStream);
            string xml = HttpUtility.UrlDecode(stream.ReadToEnd());

             if (writer != null)
            {
                writer.Info(Request, ConfigurationHelper.DebugInformation, "SetCar -> " + xml);
            }

             return new HttpResponseMessage(_setCarService.PasrsePostResponse(xml));
        }
    }
}
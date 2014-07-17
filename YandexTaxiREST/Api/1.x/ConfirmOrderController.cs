using System;
using System.Net.Http;
using System.Web.Http.Tracing;
using Business.Services.Interfaces;
using Core.Configuration.Helpers;
using YandexTaxiREST.Infrastructure.Base;

namespace YandexTaxiREST.WebAPI
{
    public class ConfirmOrderController : WebAPIBaseController
    {
        private readonly IRequestCarService _requestCarService;

        public ConfirmOrderController(IRequestCarService requestCarService)
        {
            _requestCarService = requestCarService;
        }

        // Get api/values
        public HttpResponseMessage Get(String orderId, String driverId)
        {
             if (writer != null)
            {
                writer.Info(Request, ConfigurationHelper.DebugInformation, String.Format("ConfirmOrder -> {0} {1}", orderId, driverId));
            }

            return new HttpResponseMessage(_requestCarService.ComfirmOrderFromTaxiService(orderId, driverId));
        }

    }
}
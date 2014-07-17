using System;
using System.Web.Http.Tracing;
using Business.Services.Interfaces;
using Core.Configuration.Helpers;
using YandexTaxiREST.Infrastructure.Base;
using System.Net.Http;

namespace YandexTaxiREST.WebAPI
{
    public class UpdateOrderStatusController : WebAPIBaseController
    {
        private readonly IRequestCarService _requestCarService;

        public UpdateOrderStatusController(IRequestCarService requestCarService)
        {
            _requestCarService = requestCarService;
        }

        // Get api/values
        public HttpResponseMessage Get(String orderId, String status, String newcar = null, String extra = null)
        {
            if (writer != null)
            {
                writer.Info(Request, ConfigurationHelper.DebugInformation, String.Format("UpdateOrderStatus -> {0} {1} {2} {3}", orderId, status, newcar ?? String.Empty, extra ?? String.Empty));
            }

            return new HttpResponseMessage(_requestCarService.UpdateOrderStatus(orderId, status, newcar, extra));
        }

    }
}
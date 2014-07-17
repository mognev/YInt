using System;
using System.Net.Http;
using System.Web.Http.Tracing;
using Business.Services.Interfaces;
using Core.Configuration.Helpers;
using YandexTaxiREST.Infrastructure.Base;

namespace YandexTaxiREST.WebAPI
{
    public class CancelRequestController : WebAPIBaseController
    {
        private readonly IRequestCarService _requestCarService;

        public CancelRequestController(IRequestCarService requestCarService)
        {
            _requestCarService = requestCarService;
        }

        // POST api/values
        public HttpResponseMessage Get(String orderid, String reason)
        {
            if (writer != null)
            {
                writer.Info(Request, ConfigurationHelper.DebugInformation, String.Format("CancelRequest -> {0} {1}", orderid, reason));
            }

            return new HttpResponseMessage(_requestCarService.CancelOrder(orderid, reason));
        }
    }
}
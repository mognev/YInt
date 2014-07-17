using System;
using System.Net.Http;
using System.Text;
using System.Web.Http.Tracing;
using Business.Services.Interfaces;
using Core.Configuration.Helpers;
using YandexTaxiREST.Infrastructure.Base;

namespace YandexTaxiREST.WebAPI
{
    public class CostController : WebAPIBaseController
    {
        private readonly ICostService _costService;
        private readonly IOrderService _orderService;

        public CostController(ICostService costService, IOrderService orderService)
        {
            _costService = costService;
            _orderService = orderService;
        }

        //public void Get()
        //{
        //    if (writer != null)
        //    {
        //        writer.Info(Request, ConfigurationHelper.DebugInformation, "Get the list of products.");
        //    }

        //    var o = _orderService.GetOrderById("43098ecef32848f1b9d76133eb54c4bb");
        //    int d = 1;
        //}

        //Get api/values
        public HttpResponseMessage Get(String time, String route)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    _costService.GetCost(time, route),
                    Encoding.UTF8,
                    "text/xml"
                )
            };
        }
    }
}
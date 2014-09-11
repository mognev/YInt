using System;
using System.Net.Http;
using System.Text;
using Business.Services.Interfaces;
using Core.Extension.XmlConverter;
using YandexTaxiREST.Infrastructure.Base;

namespace YandexTaxiREST.WebAPI
{
    public class TarifsController : WebAPIBaseController
    {
        private readonly ITarifService _tarifService;

        public TarifsController(ITarifService tarifService)
        {
            _tarifService = tarifService;
        }

        // GET api/values
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    _tarifService.GetTarifs().ToXmlTarifString(),
                    Encoding.UTF8,
                    "text/xml"
                )
            };
        }

    }
}
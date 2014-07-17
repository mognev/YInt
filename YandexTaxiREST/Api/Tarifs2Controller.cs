using System.Net.Http;
using System.Text;
using Business.Services.Interfaces;
using Core.Extension.XmlConverter;
using YandexTaxiREST.Infrastructure.Base;
using System.Diagnostics;

namespace YandexTaxiREST.WebAPI
{
    public class Tarifs2Controller : WebAPIBaseController
    {
        private readonly ITarifService _tarifService;

        public Tarifs2Controller(ITarifService tarifService)
        {
            _tarifService = tarifService;
        }

        // GET api/values
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    _tarifService.GetTarifs().ToXmlTarif2String(),
                    Encoding.UTF8,
                    "text/xml"
                )
            };
        }
    }
}
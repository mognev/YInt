using System;
using System.Net.Http;
using System.Text;
using Business.Services.Interfaces;
using Core.Extension.XmlConverter;
using YandexTaxiREST.Infrastructure.Base;

namespace YandexTaxiREST.WebAPI
{
    public class DriversController : WebAPIBaseController
    {
        private readonly IDriverService _driverService;
        private readonly ITarifService _tarifService;

        public DriversController(IDriverService driverService, ITarifService tarifService)
        {
            _driverService = driverService;
            _tarifService = tarifService;
        }

        // GET api/values
        public HttpResponseMessage Get()
        {
            String tarif = String.Empty;
            var firstTarif = _tarifService.GetFirstTarif();
            if (firstTarif != null)
            {
                tarif = firstTarif.Id.ToString();
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    _driverService.GetDrivers().ToXmlDriverString("tarif"),
                    Encoding.UTF8,
                    "text/xml"
                )
            };
        }
    }
}
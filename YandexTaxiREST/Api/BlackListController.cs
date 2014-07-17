using System.Net.Http;
using System.Text;
using Business.Services.Interfaces;
using Core.Extension.XmlConverter;
using YandexTaxiREST.Infrastructure.Base;

namespace YandexTaxiREST.WebAPI
{
    public class BlackListController : WebAPIBaseController
    {
        private readonly IBlackListService _blackListService;

        public BlackListController(IBlackListService blackListService)
        {
            _blackListService = blackListService;
        }

        // GET api/values
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    _blackListService.GetBlackList().ToXmlBlackListString(),
                    Encoding.UTF8,
                    "text/xml"
                )
            };
        }
    }
}
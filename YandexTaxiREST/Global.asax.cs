using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Core.Configuration.Helpers;
using YandexTaxiREST.App_Start;
using YandexTaxiREST.Infrastructure.Trace;
using YandexTaxiREST.WebAPI;

namespace YandexTaxiREST
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ContainerConfig.RegisterDependencies();
            AutomapperConfig.RegisterAutomapper();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            if (!ConfigurationHelper.CancelSchedule)
            {
                Core.Scheduler.EngineContext.Initialize();
            }

            if (ConfigurationHelper.DebugMode)
            {
                TraceConfig.Register(GlobalConfiguration.Configuration);
            }
        }
    }
}
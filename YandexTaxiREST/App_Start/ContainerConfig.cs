using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using Business.Services;
using Business.Services.Interfaces;
using Core.Db.Interfaces;
using Core.DB;
using Core.Instructure;
using Microsoft.Practices.Unity;
using YandexTaxiREST.Infrastructure.Factories;

namespace YandexTaxiREST.WebAPI
{
    public class ContainerConfig
    {
        public static UnityContainer Container { get; set; }

        public static void RegisterDependencies()
        {
            Container = new UnityContainer();
            MapTypes(Container);

            // Set resolver to MVC.
            UnityControllerActivator controllerActivator = new UnityControllerActivator(Container);
            ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory(controllerActivator));

            // Set resolver to WebApi.
            UnityHttpControllerActivator httpControllerActivator = new UnityHttpControllerActivator(Container);
            GlobalConfiguration.Configuration.Services
                .Replace(typeof(IHttpControllerActivator), httpControllerActivator);
        }

        private static void MapTypes(IUnityContainer container)
        {
            container.RegisterType<IDbContext, NavgatorTaxiObjectContext>();
            container.RegisterType(typeof(IRepository<>), typeof(EfRepository<>));
            container.RegisterType<ITarifService, TarifService>();
            container.RegisterType<IDriverService, DriverService>();
            container.RegisterType<IBlackListService, BlackListService>();
            container.RegisterType<IRequestCarService, RequestCarService>();
            container.RegisterType<ISetCarService, SetCarService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<ICostService, CostService>();
        }
    }
}
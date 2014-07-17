using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YandexTaxiREST.Infrastructure.Factories
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Microsoft.Practices.Unity;

    public class UnityControllerActivator : IControllerActivator
    {
        /// <summary>
        /// The container.
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityControllerActivator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public UnityControllerActivator(IUnityContainer container)
        {
            this._container = container;
        }

        /// <summary>
        /// When implemented in a class, creates a controller.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="controllerType">The controller type.</param>
        /// <returns>
        /// The created controller.
        /// </returns>
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            return (IController) this._container.Resolve(controllerType);
        }
    }
}
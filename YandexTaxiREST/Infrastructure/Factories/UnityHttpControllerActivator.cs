using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YandexTaxiREST.Infrastructure.Factories
{
    using System;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    using Microsoft.Practices.Unity;

    public class UnityHttpControllerActivator : IHttpControllerActivator
    {
        /// <summary>
        /// The container.
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityHttpControllerActivator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public UnityHttpControllerActivator(IUnityContainer container)
        {
            this._container = container;
        }

        /// <summary>
        /// Creates an <see cref="T:System.Web.Http.Controllers.IHttpController" /> object.
        /// </summary>
        /// <param name="request">The message request.</param>
        /// <param name="controllerDescriptor">The HTTP controller descriptor.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>
        /// An <see cref="T:System.Web.Http.Controllers.IHttpController" /> object.
        /// </returns>
        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            return (IHttpController)this._container.Resolve(controllerType);
        }
    }
}
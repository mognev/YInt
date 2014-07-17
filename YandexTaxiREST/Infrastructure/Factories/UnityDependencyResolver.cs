using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YandexTaxiREST.Infrastructure.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Microsoft.Practices.Unity;

    public class UnityDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// The container.
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// The resolver.
        /// </summary>
        private readonly IDependencyResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="resolver">The resolver.</param>
        public UnityDependencyResolver(IUnityContainer container, IDependencyResolver resolver)
        {
            this._container = container;
            this._resolver = resolver;
        }

        /// <summary>
        /// Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        /// <param name="serviceType">The type of the requested service or object.</param>
        /// <returns>
        /// The requested service or object.
        /// </returns>
        public Object GetService(Type serviceType)
        {
            try
            {
                return this._container.Resolve(serviceType);
            }
            catch
            {
                return this._resolver.GetService(serviceType);
            }
        }

        /// <summary>
        /// Resolves multiply registered services.
        /// </summary>
        /// <param name="serviceType">The type of the requested services.</param>
        /// <returns>
        /// The requested services.
        /// </returns>
        public IEnumerable<Object> GetServices(Type serviceType)
        {
            try
            {
                return this._container.ResolveAll(serviceType);
            }
            catch
            {
                return this._resolver.GetServices(serviceType);
            }
        }
    }
}
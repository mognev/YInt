namespace YandexTaxiREST.Infrastructure.Base
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Tracing;
    
    abstract public class WebAPIBaseController : ApiController
    {
        protected ITraceWriter writer
        {
            get
            {
                return Configuration.Services.GetTraceWriter();
            }
        }


        #region Protected Methods

        /// <summary>
        /// Logs exception.
        /// </summary>
        /// <param name="ex">Exception instance.</param>
        protected void LogException(Exception ex)
        {
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
        }

        #endregion Protected Methods

    }
}

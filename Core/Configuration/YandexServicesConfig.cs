using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration
{

    /// <summary>
    /// Represents an eWareConfig web.config section
    /// </summary>
    public partial class YandexServicesConfig : System.Configuration.IConfigurationSectionHandler
    {
        private System.Xml.XmlNode _scheduleTasks;

        public System.Xml.XmlNode ScheduleTasks
        {
            get { return _scheduleTasks; }
            set { _scheduleTasks = value; }
        }

        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section">Section XML node.</param>
        /// <returns>The created section handler object.</returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            YandexServicesConfig config = new YandexServicesConfig();

            config.ScheduleTasks = section.SelectSingleNode("ScheduleTasks");

            return config;
        }
    }
}

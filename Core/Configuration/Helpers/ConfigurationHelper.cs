
namespace Core.Configuration.Helpers
{
    using System;
    using System.Configuration;
    using Core.Configuration.Constant;

    public static class ConfigurationHelper
    {
        public static String ApiKey
        {
            get
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["ApiKey"]))
                {
                    throw new Exception("Not APIKEY in web.config");
                }

                return ConfigurationManager.AppSettings["ApiKey"].ToString();
            }
        }

        public static String Clid
        {
            get
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["Clid"]))
                {
                    throw new Exception("Not Clid in web.config");
                }

                return ConfigurationManager.AppSettings["Clid"].ToString();
            }
        }

        public static String UrlUpdateDriverStatus
        {
            get
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["UrlUpdateDriverStatus"]))
                {
                    throw new Exception("Not UrlUpdateDriverStatus in web.config");
                }

                return ConfigurationManager.AppSettings["UrlUpdateDriverStatus"].ToString();
            }
        }


        public static String UrlUpdateDriverTrek
        {
            get
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["UrlUpdateDriverTrek"]))
                {
                    throw new Exception("Not UrlUpdateDriverTrek in web.config");
                }

                return ConfigurationManager.AppSettings["UrlUpdateDriverTrek"].ToString();
            }
        }

        public static String UrlConfirmationOrder
        {
            get
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["UrlConfirmationOrder"]))
                {
                    throw new Exception("Not UrlConfirmationOrder in web.config");
                }

                return ConfigurationManager.AppSettings["UrlConfirmationOrder"].ToString();
            }
        }

        public static String UrlUpdateStatusOrder
        {
            get
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["UrlUpdateStatusOrder"]))
                {
                    throw new Exception("Not UrlUpdateStatusOrder in web.config");
                }

                return ConfigurationManager.AppSettings["UrlUpdateStatusOrder"].ToString();
            }
        }


        public static String UrlFeedbacks
        {
            get
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["UrlFeedbacks"]))
                {
                    throw new Exception("Not UrlFeedbacks in web.config");
                }

                return ConfigurationManager.AppSettings["UrlFeedbacks"].ToString();
            }
        }

        public static String UrlEmulateConfirmOrderFromTaxi
        {
            get
            {
                return ConfigurationManager.AppSettings["UrlEmulateConfirmOrderFromTaxi"].ToString();
            }
        }

        public static String UrlEmulateUpdateOrderStatus
        {
            get
            {
                return ConfigurationManager.AppSettings["UrlEmulateUpdateOrderStatus"].ToString();
            }
        }

        public static Boolean CancelSchedule
        {
            get
            {
                Boolean _cancelSchedule;

                if (Boolean.TryParse(ConfigurationManager.AppSettings["CancelSchedule"], out _cancelSchedule))
                {
                    return _cancelSchedule;
                }

                return DefaultConfigarationConstant.CancelSchedule;
            }
        }

        public static Boolean DebugMode
        {
            get
            {
                Boolean _debugMode;

                if (Boolean.TryParse(ConfigurationManager.AppSettings["DebugMode"], out _debugMode))
                {
                    return _debugMode;
                }

                return DefaultConfigarationConstant.DebugMode;
            }
        }


        public static String DebugInformation
        {
            get
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["DebugInformation"]))
                {
                    return DefaultConfigarationConstant.DebugInformation;
                }

                return ConfigurationManager.AppSettings["DebugInformation"].ToString();
            }
        }

        public static Int32 FreeTime
        {
            get
            {
                Int32 _value = 0;
                if (Int32.TryParse(ConfigurationManager.AppSettings["FreeTime"], out _value))
                {
                    return _value;
                }

                return DefaultConfigarationConstant.FreeTime;
                
            }
        }


        public static Int32 StopSpeed
        {
            get
            {
                Int32 _value = 0;
                if (Int32.TryParse(ConfigurationManager.AppSettings["StopSpeed"], out _value))
                {
                    return _value;
                }

                return DefaultConfigarationConstant.StopSpeed;

            }
        }

        public static String TaxiSite
        {
            get
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["TaxiSite"]))
                {
                    return null;
                }

                return ConfigurationManager.AppSettings["TaxiSite"].ToString();
            }
        }

        public static String TaxiName
        {
            get
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["TaxiName"]))
                {
                    return null;
                }

                return ConfigurationManager.AppSettings["TaxiName"].ToString();
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Core.Extension.Entity;
using Core.Domain;

namespace Core.Extension.Converter
{
    public static class Converter
    {
        public static Address ToAddressFromXmlNode(XmlNode node)
        {

            Address address = new Address();
            try
            {
                XmlNode fullname = node.SelectSingleNode("FullName");
                address.FullName = fullname.InnerText.Trim();
            }
            catch (XPathException)
            {
                return null;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            try
            {
                XmlNode shortName = node.SelectSingleNode("ShortName");
                address.ShortName = shortName.InnerText.Trim();
                XmlNode lon = node.SelectSingleNode("Point/Lon");
                Decimal valueTry;
                if (Decimal.TryParse(lon.InnerText.Trim(), NumberStyles.Any, new CultureInfo("en-GB"), out valueTry))
                {
                    address.Lon = valueTry;
                }
                XmlNode lat = node.SelectSingleNode("Point/Lat");
                if (Decimal.TryParse(lat.InnerText.Trim(), NumberStyles.Any, new CultureInfo("en-GB"), out valueTry))
                {
                    address.Lat = valueTry;
                }

                XmlNode localityName = node.SelectSingleNode("Country/Locality/LocalityName");
                if (localityName != null)
                {
                    address.LocalityName = localityName.InnerText.Trim();
                }

                XmlNode thoroughfareName = node.SelectSingleNode("Country/Locality/Thoroughfare/ThoroughfareName");
                if (thoroughfareName != null)
                {
                    address.StreetName = thoroughfareName.InnerText.Trim();
                }

                XmlNode premiseNumber = node.SelectSingleNode("Country/Locality/Thoroughfare/Premise/PremiseNumber");
                if (premiseNumber != null)
                {
                    address.PremiseNumber = premiseNumber.InnerText.Trim();
                }

                XmlNode porchNumber = node.SelectSingleNode("Country/Locality/Thoroughfare/Premise/PorchNumber");
                if (porchNumber != null)
                {
                    address.PorchNumber = porchNumber.InnerText.Trim();
                }
            }
            catch
            {
                return address;
            }

            return address;
        }

        public static Order ToRequirements(XmlNodeList nodeList, Order order)
        {
            foreach (XmlNode item in nodeList)
            {
                String attr = item.Attributes["name"].Value;
                String value = item.InnerText;
                AdapterRequirementsToOrder(order, attr, value);
            }
            return order;
        }


        private static void AdapterRequirementsToOrder(Order order, String field, String value)
        {
            Boolean val = false;
            if (String.IsNullOrEmpty(value.Trim()))
                return;

            if (value.Trim().ToLower() == "yes")
            {
                val = true;
            } 

            switch (field)
            {
                case "has_conditioner":
                    order.Rq_has_conditioner = val;
                    break;
                case "no_smoking":
                    order.Rq_no_smoking = val;
                    break;
                case "child_chair":
                    order.Rq_child_chair = val;
                    break;
                case "animal_transport":
                    order.Rq_animal_transport = val;
                    break;
                case "universal":
                    order.Rq_universal = val;
                    break;
                case "wifi":
                    order.Rq_wifi = val;
                    break;
                case "check":
                    order.Rq_check = val;
                    break;
                case "card":
                    order.Rq_card = val;
                    break;
                case "yamoney":
                    order.Rq_yamoney = val;
                    break;
                case "newspaper":
                    order.Rq_newspaper = val;
                    break;
            }
        }

    }
}

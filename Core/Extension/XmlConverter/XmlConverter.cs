using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml;
using Core.Configuration.Helpers;
using Core.Domain;

namespace Core.Extension.XmlConverter
{
    public static class XmlConverter
    {
        private static List<String> _weekDays = new List<String>()
        {
            "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", "Holiday"
        };

        public static String ToXmlTarifString(this IEnumerable<Tarif> tarifsList)
        {

            XmlDocument document = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement tariffs = document.CreateElement("Tariffs");

            XmlElement tarif = document.CreateElement("Tariff");
            XmlElement id = document.CreateElement("Id");
            id.InnerText = "tarif";
            XmlElement name = document.CreateElement("Name");
            name.InnerText = "Основной тариф"; // TODO Move to file resource
            XmlElement description = document.CreateElement("Description");
            Int32 count = 0;
            foreach (var item in tarifsList)
            {
                count++;
                String start = item.TimeS.Value.ToString();
                if (item.TimeS.Value.ToString().Length == 1)
                {
                    start = String.Join("", "0", item.TimeS.Value.ToString());
                }

                String end = (item.TimePo.Value - 1).ToString();
                if (item.TimePo.Value.ToString().Length == 1)
                {
                    end = String.Join("", "0", (item.TimePo.Value - 1).ToString());
                }
                String timeStart = String.Join(":", start, "00");
                String timeEnd = String.Join(":", end, "59");

                XmlElement interval = document.CreateElement("Interval");
                interval.SetAttribute("name", "День");

                XmlElement schedule = document.CreateElement("Schedule");
                XmlElement span = document.CreateElement("Span");
                XmlElement spanInterval = document.CreateElement("Interval");
                spanInterval.SetAttribute("start", timeStart);
                spanInterval.SetAttribute("end", timeEnd);
                span.AppendChild(spanInterval);
                for (int i = 1; i <= 7; i++)
                {
                    XmlElement weekday = document.CreateElement("Weekday");
                    weekday.InnerText = i.ToString();
                    span.AppendChild(weekday);
                }

                schedule.AppendChild(span);

                //if (count < 2)
                //{
                interval.AppendChild(schedule);
                //}
                XmlElement special = document.CreateElement("Special");
                interval.AppendChild(special);
                XmlElement city = document.CreateElement("City");
                XmlElement minPrice = document.CreateElement("MinPrice");
                XmlElement currency = document.CreateElement("Currency");
                currency.InnerText = "руб";
                XmlElement included = document.CreateElement("Included");

                if (!item.MinimalSum.HasValue || item.MinimalSum.Value == 0)
                {
                    if (item.CallCar.HasValue)
                    {
                        minPrice.InnerText = ((Int32)item.CallCar.Value).ToString();
                    }
                    included.InnerText = String.Join(" ", "0", "км");
                }
                else
                {
                    minPrice.InnerText = ((Int32)item.MinimalSum.Value).ToString();
                    if (item.MinimalKM.HasValue)
                    {
                        included.InnerText = String.Join(" ", item.MinimalKM.Value, "км");
                    }
                }

                XmlElement other = document.CreateElement("Other");
                try
                {
                    other.InnerText = String.Join(" ", "По городу", ((Int32)item.Km.Value).ToString(), "руб/км,", "за городом",
                                                  item.Kmzagorod.Value.ToString(), "руб/км");
                }
                catch (Exception)
                {
                    other.InnerText = String.Empty;
                }

                city.AppendChild(minPrice);
                city.AppendChild(currency);
                city.AppendChild(included);
                city.AppendChild(other);
                interval.AppendChild(city);
                description.AppendChild(interval);
                tarif.AppendChild(id);
                tarif.AppendChild(name);

                XmlElement commonOverPrice = document.CreateElement("CommonOverPrice");

                XmlElement unit = document.CreateElement("Unit");
                unit.SetAttribute("name", "Подача");
                unit.InnerText = String.Join(" ", ((Int32)item.CallCar.Value).ToString(), "руб");
                commonOverPrice.AppendChild(unit);

                XmlElement unit1 = document.CreateElement("Unit");
                unit1.SetAttribute("name", "Тариф по городу");
                unit1.InnerText = String.Join(" ", ((Int32)item.Km.Value).ToString(), "руб/км");
                commonOverPrice.AppendChild(unit1);

                XmlElement unit2 = document.CreateElement("Unit");
                unit2.SetAttribute("name", "Тариф за городом");
                unit2.InnerText = String.Join(" ", ((Int32)item.Kmzagorod.Value).ToString(), "руб/км");
                commonOverPrice.AppendChild(unit2);

                XmlElement unit3 = document.CreateElement("Unit");
                unit3.SetAttribute("name", "Ожидание");
                unit3.InnerText = String.Join(" ", ((Int32)item.Ostanovka.Value).ToString(), "руб/мин");
                commonOverPrice.AppendChild(unit3);

                description.AppendChild(interval);
            }

            tarif.AppendChild(id);
            tarif.AppendChild(name);
            tarif.AppendChild(description);
            tariffs.AppendChild(tarif);
            document.AppendChild(tariffs);
            document.InsertBefore(xmldecl, tariffs);
            return document.OuterXml;

        }

        public static String ToXmlTarif2String(this IEnumerable<Tarif> tarifsList)
        {

            XmlDocument document = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement tariffs = document.CreateElement("Tariffs");
            tariffs.SetAttribute("Version", "1.0");

            XmlElement tarif = document.CreateElement("Tariff");
            XmlElement id = document.CreateElement("Id");
            id.InnerText = "tarif";
            tarif.AppendChild(id);

            XmlElement name = document.CreateElement("Name");
            name.SetAttribute("Lang", "ru");
            name.InnerText = "Основной тариф";
            tarif.AppendChild(name);

            XmlElement intervals = document.CreateElement("Intervals");
            XmlElement tariffChoise = document.CreateElement("TariffChoice");
            tariffChoise.InnerText = "start";
            intervals.AppendChild(tariffChoise);

            foreach (var item in tarifsList)
            {
                XmlElement interval = document.CreateElement("Interval");

                XmlElement schedule = document.CreateElement("Schedule");
                XmlElement span = document.CreateElement("Span");
                XmlElement days = document.CreateElement("Days");

                foreach (String day in _weekDays)
                {
                    XmlElement itemDays = document.CreateElement("Item");
                    itemDays.InnerText = day;
                    days.AppendChild(itemDays);
                }

                span.AppendChild(days);

                String start = item.TimeS.Value.ToString();
                if (item.TimeS.Value.ToString().Length == 1)
                {
                    start = String.Join("", "0", item.TimeS.Value.ToString());
                }

                String end = (item.TimePo.Value - 1).ToString();
                if (item.TimePo.Value.ToString().Length == 1)
                {
                    end = String.Join("", "0", (item.TimePo.Value - 1).ToString());
                }
                String timeStart = String.Join(":", start, "00");
                String timeEnd = String.Join(":", end, "59");

                String periond = String.Join("-", timeStart, timeEnd);

                XmlElement timeInterval = document.CreateElement("TimeInterval");
                XmlElement startTimeInterval = document.CreateElement("Start");
                startTimeInterval.InnerText = timeStart;

                XmlElement endTimeInterval = document.CreateElement("End");
                endTimeInterval.InnerText = timeEnd;
                timeInterval.AppendChild(startTimeInterval);
                timeInterval.AppendChild(endTimeInterval);


                span.AppendChild(timeInterval);

                schedule.AppendChild(span);
                interval.AppendChild(schedule);

                XmlElement freeroute = document.CreateElement("FreeRoute");
                XmlElement services = document.CreateElement("Services");
                XmlElement service = document.CreateElement("Service");
                service.SetAttribute("Type", "taximeter");
                service.SetAttribute("CalcRule", "sum");
                XmlElement taximeterCalc = document.CreateElement("TaximeterCalc");

                XmlElement minPrice = document.CreateElement("MinPrice");
                minPrice.SetAttribute("Type", "once");
                String included;
                if (!item.MinimalSum.HasValue || item.MinimalSum.Value == 0)
                {
                    if (item.CallCar.HasValue)
                    {
                        minPrice.InnerText = ((Int32)item.CallCar.Value).ToString();
                    }
                    included = String.Join(" ", "0", "км");
                }
                else
                {
                    minPrice.InnerText = ((Int32)item.MinimalSum.Value).ToString();
                    if (item.MinimalKM.HasValue)
                    {
                        included = String.Join(" ", item.MinimalKM.Value, "км");
                    }
                }

                taximeterCalc.AppendChild(minPrice);

                XmlElement stopSpeed = document.CreateElement("StopSpeed");
                stopSpeed.SetAttribute("Unit", "km/h");
                stopSpeed.InnerText = ConfigurationHelper.StopSpeed.ToString();
                taximeterCalc.AppendChild(stopSpeed);

                #region gorog
                XmlElement meter = document.CreateElement("Meter");
                meter.SetAttribute("Type", "driving_distance");

                XmlElement areas = document.CreateElement("Areas");
                XmlElement areaItem = document.CreateElement("Item");
                areaItem.InnerText = "city";
                areas.AppendChild(areaItem);
                meter.AppendChild(areas);

                XmlElement price = document.CreateElement("Price");
                if (item.Km.HasValue)
                {
                    price.InnerText = item.Km.Value.ToString(CultureInfo.InvariantCulture);
                }
                meter.AppendChild(price);

                XmlElement per = document.CreateElement("Per");
                per.SetAttribute("Unit", "kilometer");
                per.InnerText = "1";
                meter.AppendChild(per);

                taximeterCalc.AppendChild(meter);
                #endregion

                #region zagorog
                meter = document.CreateElement("Meter");
                meter.SetAttribute("Type", "driving_distance");

                areas = document.CreateElement("Areas");
                areaItem = document.CreateElement("Item");
                areaItem.InnerText = "suburb";
                areas.AppendChild(areaItem);
                meter.AppendChild(areas);

                price = document.CreateElement("Price");
                if (item.Kmzagorod.HasValue)
                {
                    price.InnerText = item.Kmzagorod.Value.ToString(CultureInfo.InvariantCulture);
                }
                meter.AppendChild(price);

                per = document.CreateElement("Per");
                per.SetAttribute("Unit", "kilometer");
                per.InnerText = "1";
                meter.AppendChild(per);

                taximeterCalc.AppendChild(meter);
                #endregion

                #region probki
                meter = document.CreateElement("Meter");
                meter.SetAttribute("Type", "idle_time");

                areas = document.CreateElement("Areas");
                areaItem = document.CreateElement("Item");
                areaItem.InnerText = "city";
                areas.AppendChild(areaItem);
                areaItem = document.CreateElement("Item");
                areaItem.InnerText = "suburb";
                areas.AppendChild(areaItem);
                meter.AppendChild(areas);

                price = document.CreateElement("Price");
                if (item.Ostanovka.HasValue)
                {
                    price.InnerText = item.Ostanovka.Value.ToString(CultureInfo.InvariantCulture);
                }
                meter.AppendChild(price);

                per = document.CreateElement("Per");
                per.SetAttribute("Unit", "minute");
                per.InnerText = "1";
                meter.AppendChild(per);

                taximeterCalc.AppendChild(meter);
                #endregion

                service.AppendChild(taximeterCalc);
                services.AppendChild(service);
                service = document.CreateElement("Service");
                service.SetAttribute("Type", "waiting");
                XmlElement freeTime = document.CreateElement("FreeTime");
                freeTime.SetAttribute("Unit", "minute");
                freeTime.InnerText = ConfigurationHelper.FreeTime.ToString();
                service.AppendChild(freeTime);
                services.AppendChild(service);
                freeroute.AppendChild(services);
                interval.AppendChild(freeroute);
                intervals.AppendChild(interval);
            }

            tarif.AppendChild(intervals);
            tariffs.AppendChild(tarif);
            document.AppendChild(tariffs);
            document.InsertBefore(xmldecl, tariffs);
            return document.OuterXml;

        }


        public static String ToXmlDriverString(this IEnumerable<Driver> driverList, String tarif)
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement cars = document.CreateElement("Cars");

            foreach (var item in driverList)
            {
                XmlElement car = document.CreateElement("Car");
                XmlElement uuid = document.CreateElement("Uuid");
                uuid.InnerText = item.ID_DRIVER.ToString();
                car.AppendChild(uuid);
                if (!String.IsNullOrEmpty(ConfigurationHelper.Clid))
                {
                    XmlElement realClid = document.CreateElement("RealClid");
                    realClid.InnerText = ConfigurationHelper.Clid;
                    car.AppendChild(realClid);
                    if (!String.IsNullOrEmpty(ConfigurationHelper.TaxiName))
                    {
                        XmlElement realName = document.CreateElement("RealName");
                        realName.InnerText = ConfigurationHelper.TaxiName;
                        car.AppendChild(realName);
                    }

                    if (!String.IsNullOrEmpty(ConfigurationHelper.TaxiSite))
                    {
                        XmlElement realWeb = document.CreateElement("RealWeb");
                        realWeb.InnerText = ConfigurationHelper.TaxiSite;
                        car.AppendChild(realWeb);
                    }
                }

                XmlElement tariff = document.CreateElement("Tariff");
                tariff.InnerText = tarif;
                car.AppendChild(tariff);
                //TODO Driver Details
                XmlElement driverDetails = document.CreateElement("DriverDetails");
                XmlElement displayName = document.CreateElement("DisplayName");
                try
                {
                    displayName.InnerText = String.Format("{0} {1} {2}", item.Fam.Trim(), item.Im.Trim(), item.Otch.Trim());
                }
                catch
                {
                    displayName.InnerText = String.Empty;
                    if (!String.IsNullOrEmpty(item.Fam))
                    {
                        displayName.InnerText += item.Fam.Trim();
                    }
                    if (!String.IsNullOrEmpty(item.Im))
                    {
                        displayName.InnerText += " " + item.Im.Trim();
                    }
                    if (!String.IsNullOrEmpty(item.Otch))
                    {
                        displayName.InnerText += " " + item.Otch.Trim();
                    }
                }
                driverDetails.AppendChild(displayName);
                XmlElement phone = document.CreateElement("Phone");
                phone.InnerText = item.Tel;
                driverDetails.AppendChild(phone);
                XmlElement driverAge = document.CreateElement("Age");
                if (item.dtB.HasValue)
                {
                    driverAge.InnerText = item.dtB.Value.Year.ToString();
                }
                driverDetails.AppendChild(driverAge);
                XmlElement driverLicense = document.CreateElement("DriverLicense");
                if (!String.IsNullOrEmpty(item.UD))
                {
                    driverLicense.InnerText = item.UD;
                }
                driverDetails.AppendChild(driverLicense);
                car.AppendChild(driverDetails);

                XmlElement carDetails = document.CreateElement("CarDetails");
                XmlElement model = document.CreateElement("Model");
                model.InnerText = item.Car;
                carDetails.AppendChild(model);

                if (item.CarYear.HasValue && item.CarYear.Value != 1900)
                {
                    XmlElement carage = document.CreateElement("Age");
                    carage.InnerText = item.CarYear.Value.ToString();
                    carDetails.AppendChild(carage);
                }

                XmlElement color = document.CreateElement("Color");
                color.InnerText = item.ColorCar;
                carDetails.AppendChild(color);

                XmlElement carNumber = document.CreateElement("CarNumber");
                carNumber.InnerText = item.NumberCar;
                carDetails.AppendChild(carNumber);

                if (!String.IsNullOrEmpty(item.LicenceKart))
                {
                    XmlElement permit = document.CreateElement("Permit");
                    permit.InnerText = item.LicenceKart;
                    carDetails.AppendChild(permit);
                }

                XmlElement require = document.CreateElement("Require");
                require.SetAttribute("name", "has_conditioner");
                if (item.IsCondition.HasValue && item.IsCondition.Value == 1)
                {
                    require.InnerText = "yes";
                }
                else
                {
                    require.InnerText = "no";
                }
                carDetails.AppendChild(require);

                XmlElement require2 = document.CreateElement("Require");
                require2.SetAttribute("name", "no_smoking");
                if (item.IsSmoke.HasValue && item.IsSmoke.Value == 1)
                {
                    require2.InnerText = "no";
                }
                else
                {
                    require2.InnerText = "yes";
                }
                carDetails.AppendChild(require2);

                XmlElement require4 = document.CreateElement("Require");
                require4.SetAttribute("name", "child_chair");
                require4.InnerText = "no";
                carDetails.AppendChild(require4);

                XmlElement require5 = document.CreateElement("Require");
                require5.SetAttribute("name", "animal_transport");
                require5.InnerText = "no";
                carDetails.AppendChild(require5);

                XmlElement require6 = document.CreateElement("Require");
                require6.SetAttribute("name", "universal");
                require6.InnerText = "no";
                carDetails.AppendChild(require6);

                XmlElement require7 = document.CreateElement("Require");
                require7.SetAttribute("name", "wifi");
                require7.InnerText = "no";
                carDetails.AppendChild(require7);

                XmlElement require8 = document.CreateElement("Require");
                require8.SetAttribute("name", "check");
                require8.InnerText = "no";
                carDetails.AppendChild(require8);

                XmlElement require9 = document.CreateElement("Require");
                require9.SetAttribute("name", "card");
                require9.InnerText = "no";
                carDetails.AppendChild(require9);

                XmlElement require10 = document.CreateElement("Require");
                require10.SetAttribute("name", "yamoney");
                require10.InnerText = "no";
                carDetails.AppendChild(require10);

                XmlElement require11 = document.CreateElement("Require");
                require11.SetAttribute("name", "newspaper");
                require11.InnerText = "no";
                carDetails.AppendChild(require11);

                car.AppendChild(carDetails);
                cars.AppendChild(car);
            }


            document.AppendChild(cars);
            document.InsertBefore(xmldecl, cars);
            return document.OuterXml;
        }

        public static String ToXmlBlackListString(this IEnumerable<BlackPhone> blackPhoneList)
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement blacklist = document.CreateElement("Blacklist");
            foreach (var item in blackPhoneList)
            {
                XmlElement entry = document.CreateElement("Entry");
                entry.SetAttribute("phone", item.Phone);
                blacklist.AppendChild(entry);
            }
            document.AppendChild(blacklist);
            document.InsertBefore(xmldecl, blacklist);
            return document.OuterXml;
        }

        public static String ToXmlDriverStatusString(this IEnumerable<DriverShedule> driverStatusList)
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement carsStatus = document.CreateElement("CarsStatus");
            XmlElement cars = document.CreateElement("Cars");
            foreach (var item in driverStatusList)
            {
                XmlElement car = document.CreateElement("Car");
                car.SetAttribute("uuid", item.ID_DRIVER.ToString());
                car.SetAttribute("status", item.Y_Status);
                cars.AppendChild(car);
            }
            carsStatus.AppendChild(cars);
            document.AppendChild(carsStatus);
            document.InsertBefore(xmldecl, carsStatus);
            return document.OuterXml;
        }

        public static String ToXmlDriverTrekString(this IEnumerable<DriverShedule> driverTrekList)
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement tracks = document.CreateElement("tracks");
            tracks.SetAttribute("clid", ConfigurationHelper.Clid);
            foreach (var item in driverTrekList)
            {
                if (item.LastSession.HasValue)
                {
                    DateTime dateNow = DateTime.UtcNow;
                    DateTime date = DateTime.UtcNow;
                    date = TimeZoneInfo.ConvertTimeToUtc(item.LastSession.Value, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));

                    // не передаем на яндекс старые данные
                    if ((dateNow - date).TotalMinutes < 5)
                    {
                        var culture = CultureInfo.CreateSpecificCulture("en-US");
                        XmlElement track = document.CreateElement("track");
                        track.SetAttribute("uuid", item.ID_DRIVER.ToString());
                        XmlElement point = document.CreateElement("point");
                        point.SetAttribute("latitude", (item.PosY.HasValue) ? item.PosY.Value.ToString(culture) : String.Empty);
                        point.SetAttribute("longitude", (item.PosX.HasValue) ? item.PosX.Value.ToString(culture) : String.Empty);
                        point.SetAttribute("avg_speed", (item.CurrentSpeed.HasValue) ? item.CurrentSpeed.Value.ToString() : String.Empty);
                        point.SetAttribute("direction", "0");
                        point.SetAttribute("time", (item.CurrentSpeed.HasValue) ? date.ToString("ddMMyyy:HHmmss") : String.Empty);
                        point.SetAttribute("category", "n");
                        track.AppendChild(point);
                        tracks.AppendChild(track);
                    }
                }
            }

            document.AppendChild(tracks);
            document.InsertBefore(xmldecl, tracks);
            return document.OuterXml;
        }

    }
}
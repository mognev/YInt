using System;

namespace Core.Extension.Entity
{
    public class Address
    {
        public String FullName { get; set; }
        public String ShortName { get; set; }
        public Decimal Lon { get; set; }
        public Decimal Lat { get; set; }
        public String LocalityName { get; set; }
        public String StreetName { get; set; }
        public String PremiseNumber { get; set; }
        public String PorchNumber { get; set; }
    }
}

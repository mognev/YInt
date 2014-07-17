
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Order : EntityBase
    {
        [Key]
        public Int64 Id { get; set; }
        public String OrderId { get; set; }
        public String Drivers { get; set; }
        public Boolean? Loyal { get; set; }
        public Boolean? Blacklisted { get; set; }
        public String FullName { get; set; }
        public String ShortName { get; set; }
        public Decimal? Lon { get; set; }
        public Decimal? Lat { get; set; }
        public String LocalityName { get; set; }
        public String StreetName { get; set; }
        public String PremiseNumber { get; set; }
        public String PorchNumber { get; set; }

        public String D_FullName { get; set; }
        public String D_ShortName { get; set; }
        public Decimal? D_Lon { get; set; }
        public Decimal? D_Lat { get; set; }
        public String D_LocalityName { get; set; }
        public String D_StreetName { get; set; }
        public String D_PremiseNumber { get; set; }
        public String D_PorchNumber { get; set; }

        public DateTime? BookingTime { get; set; }
        public String BookingTimeType { get; set; }
        public Int32? id_call { get; set; }
        public Int32? driver_id { get; set; }

        public String Phone { get; set; }
        public String ClientName { get; set; }
        public String Comments { get; set; }

        public Boolean? IsDeleted { get; set; }
        public Boolean? YandexAcceptOrder { get; set; }

        public String CancelRequest { get; set; }
        public String OrderStatus { get; set; }

        public Boolean? Rq_has_conditioner{ get; set; }
        public Boolean? Rq_no_smoking { get; set; }
        public Boolean? Rq_child_chair { get; set; }
        public Boolean? Rq_animal_transport { get; set; }
        public Boolean? Rq_universal { get; set; }
        public Boolean? Rq_wifi { get; set; }
        public Boolean? Rq_check { get; set; }
        public Boolean? Rq_card { get; set; }
        public Boolean? Rq_yamoney { get; set; }
        public Boolean? Rq_newspaper { get; set; }

        public Boolean? PreOrder { get; set; }

        #region navigation property
        public virtual ICollection<OrderDrivers> OrderDrivers { get; set; }
        #endregion
    }
}

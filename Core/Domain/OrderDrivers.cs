
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    public class OrderDrivers : EntityBase
    {
        [Key]
        public Int64 Id { get; set; }
        public Int64 OrderId { get; set; }
        public String Drivers { get; set; }
        public DateTime? Dt { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

    }
}


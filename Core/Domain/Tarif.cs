
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class Tarif : EntityBase
    {
        [Key]
        public Int32 Id { get; set; }
        public Decimal? Km { get; set; }
        public Decimal? Kmzagorod { get; set; }
        public Decimal? Ostanovka { get; set; }
        public Int32 IsActive { get; set; }
        public Int32? TimeS { get; set; }
        public Int32? TimePo { get; set; }
        public Decimal? MinimalSum { get; set; }
        public Int32? MinimalKM { get; set; }
        public Decimal? MinuteCost { get; set; }
        public Decimal? CallCar { get; set; }
 
    }
}

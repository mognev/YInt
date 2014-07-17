
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class Driver : EntityBase
    {
        [Key]
        public Int32 ID_DRIVER { get; set; }
        public String Car { get; set; }
        public String NumberCar { get; set; }
        public String ColorCar { get; set; }
        public Int32? CarYear { get; set; }
        public Int32? StatusDriver { get; set; }
        public Int32 IsDeleted { get; set; }
        public Int32? IsSmoke { get; set; }
        public String LicenceKart { get; set; }
        public String Fam { get; set; }
        public String Im { get; set; }
        public String Otch { get; set; }
        public String Tel { get; set; }
        public Int32? IsCondition { get; set; }

        // Location data
        public Single? PosX { get; set; }
        public Single? PosY { get; set; }
        public DateTime? LastSession { get; set; }
        public Int32? CurrentSpeed { get; set; }
    }
}

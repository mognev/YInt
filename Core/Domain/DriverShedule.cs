
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class DriverShedule: EntityBase
    {
        [Key]
        public Int32 ID_DRIVER { get; set; }
        public String Y_Status { get; set; }
        public Single? PosX { get; set; }
        public Single? PosY { get; set; }
        public DateTime? LastSession { get; set; }
        public Int32? CurrentSpeed { get; set; }
    }
}

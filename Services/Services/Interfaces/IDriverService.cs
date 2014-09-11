namespace Business.Services.Interfaces
{
    using Core.Domain;
    using System;
    using System.Collections.Generic;

    public interface IDriverService : IDisposable
    {
        IEnumerable<Driver> GetDrivers();
        IEnumerable<DriverShedule> SheduleGetDrivers();
        Driver GetDriverById(Int32 driverId);
    }
}

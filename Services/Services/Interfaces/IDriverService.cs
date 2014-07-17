namespace Business.Services.Interfaces
{
    using Core.Domain;
    using System;
    using System.Collections.Generic;

    public interface IDriverService : IDisposable
    {
        List<Driver> GetDrivers();
        List<DriverShedule> SheduleGetDrivers();
        Driver GetDriverById(Int32 driverId);
    }
}

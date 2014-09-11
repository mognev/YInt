
namespace Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Db.Interfaces;
    using Core.DB;
    using Core.Domain;
    using Services.Interfaces;

    public class DriverService : IDriverService
    {
        private readonly IRepository<Driver> _driverRepository;
        private NavgatorTaxiObjectContext _db = new NavgatorTaxiObjectContext();


        public DriverService(IRepository<Driver> driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public IEnumerable<Driver> GetDrivers()
        {
               return _db.ExecuteStoredProcedure<Driver>("[dbo].[sp_y_GetAllDriverList]");
        }

        public IEnumerable<DriverShedule> SheduleGetDrivers()
        {
              return _db.ExecuteStoredProcedure<DriverShedule>("[dbo].[sp_y_GetDriverList]");
        }

        public Driver GetDriverById(Int32 driverId) {
            return _driverRepository.Table.FirstOrDefault(x => x.ID_DRIVER == driverId && x.IsDeleted == 0);
        }

        public void Dispose()
        {
            _db.Dispose();
            _driverRepository.Dispose();
        }
    }
}

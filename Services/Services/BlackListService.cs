
namespace Business.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Db.Interfaces;
    using Core.Domain;
    using Services.Interfaces;
    using System.Data.Entity.Infrastructure;
    using Core.DB;

    public class BlackListService : IBlackListService
    {
        private readonly IRepository<BlackPhone> _blackListRepository;
        private NavgatorTaxiObjectContext _db = new NavgatorTaxiObjectContext();

        public BlackListService(IRepository<BlackPhone> blackListRepository)
        {
            _blackListRepository = blackListRepository;
        }

        public IEnumerable<BlackPhone> GetBlackList()
        {
           return _db.ExecuteStoredProcedure<BlackPhone>("[dbo].[sp_y_GetPhoneBlockList]");
        }

        public void Dispose()
        {
            _db.Dispose();
            _blackListRepository.Dispose();
        }
    }
}


namespace Business.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Db.Interfaces;
    using Core.Domain;
    using Services.Interfaces;

    public class TarifService : ITarifService
    {
        private readonly IRepository<Tarif> _tarifRepository;

        public TarifService(IRepository<Tarif> tarifRepository)
        {
            _tarifRepository = tarifRepository;
        }

        public List<Tarif> GetTarifs()
        {
            return _tarifRepository.Table.Where(x => x.IsActive == 1).ToList();
        }

        public Tarif GetFirstTarif()
        {
            return _tarifRepository.Table.FirstOrDefault();
        }
    }
}

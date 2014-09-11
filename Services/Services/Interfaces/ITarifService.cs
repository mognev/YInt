using System.Collections.Generic;
using Core.Domain;

namespace Business.Services.Interfaces
{
    public interface ITarifService
    {
        Tarif GetFirstTarif();
        IEnumerable<Tarif> GetTarifs();
    }
}

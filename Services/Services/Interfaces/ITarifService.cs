using System.Collections.Generic;
using Core.Domain;

namespace Business.Services.Interfaces
{
    public interface ITarifService
    {
        Tarif GetFirstTarif();
        List<Tarif> GetTarifs();
    }
}

using System.Collections.Generic;
using Core.Domain;

namespace Business.Services.Interfaces
{
    public interface IBlackListService
    {
        List<BlackPhone> GetBlackList();
    }
}

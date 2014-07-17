using System;
using System.Collections.Generic;
using Core.Domain;

namespace Business.Services.Interfaces
{
    public interface ICostService
    {
        String GetCost(String time, String route);
    }
}

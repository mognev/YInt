using System;
using System.Collections.Generic;
using System.Net;
using Core.Enum;

namespace Business.Services.Interfaces
{
    public interface ISetCarService
    {
        HttpStatusCode PasrsePostResponse(String xml);
    }
}

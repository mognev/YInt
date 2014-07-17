using System;
using System.Collections.Generic;
using System.Net;
using Core.Enum;

namespace Business.Services.Interfaces
{
    public interface IRequestCarService
    {
        //FindDriverInStorage GetRequestStatusDriver(List<String> driversId);
        HttpStatusCode PasrsePostResponse(String xml);
        HttpStatusCode CancelOrder(String orderId, String reason);
        HttpStatusCode ComfirmOrderFromTaxiService(String orderId, String driverId);
        HttpStatusCode UpdateOrderStatus(String orderId, String status, String newcar = null, String extra = null);
    }
}

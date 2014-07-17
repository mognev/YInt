using System.Collections.Generic;
using Core.Domain;
using System;

namespace Business.Services.Interfaces
{
    public interface IOrderService : IDisposable
    {
        void Insert(Order entity);
        void Update(Order entity);
        void Delete(String orderId, String reason);
        Order GetOrderById(String orderId);
        Order GetOrderForConfirmation();
        Order GetConfirmationOrder();
        Order GetOrderByIdForCancel(String orderId);
    }
}


namespace Business.Services
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Core.Db.Interfaces;
    using Core.DB;
    using Core.Domain;
    using Services.Interfaces;

    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private NavgatorTaxiObjectContext _db = new NavgatorTaxiObjectContext();


        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void Insert(Order entity)
        {
            _orderRepository.Insert(entity);
        }

        public void Update(Order entity)
        {
            _orderRepository.Update(entity);
        }

        public void Delete(String orderId, String reason)
        {
            var orders = _orderRepository.Table.Where(x => x.OrderId == orderId && !x.IsDeleted.Value);
            if (orders != null)
            {
                foreach (var order in orders)
                {
                    order.IsDeleted = true;
                    order.CancelRequest = reason;
                    _orderRepository.Update(order);
                }
            }
        }

        public Order GetOrderById(String orderId)
        {
            return _orderRepository.Table.FirstOrDefault(x => x.OrderId == orderId && !x.IsDeleted.Value );
        }

        public Order GetOrderForConfirmation()
        {
            DateTime time = DateTime.Now.AddMinutes(-5);
            return _orderRepository.Table
                .FirstOrDefault(x => x.BookingTime.Value > time 
                    && !x.YandexAcceptOrder.Value 
                    && !x.IsDeleted.Value
                    && x.OrderStatus == "new");
        }

        public Order GetConfirmationOrder()
        {
            return _orderRepository.Table.Where(x => x.YandexAcceptOrder.Value && !x.IsDeleted.Value).ToList().LastOrDefault();
        }

        /// <summary>
        /// Get order always (even if deleted)
        /// </summary>
        /// <param name="orderId">String orderId</param>
        /// <returns></returns>
        public Order GetOrderByIdForCancel(String orderId)
        {
            return _orderRepository.Table.FirstOrDefault(x => x.OrderId == orderId);
        }

        public void Dispose()
        {
            _db.Dispose();
            _orderRepository.Dispose();
        }

    }
}

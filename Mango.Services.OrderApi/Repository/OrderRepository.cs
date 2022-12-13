using Mango.Services.OrderApi.Models;

namespace Mango.Services.OrderApi.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public Task<bool> AddOrder(OrderHeader orderHeader)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrderPaymentStatus(int OrderHeaderId, bool paid)
        {
            throw new NotImplementedException();
        }
    }
}

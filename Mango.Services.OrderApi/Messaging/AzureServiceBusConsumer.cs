using Azure.Messaging.ServiceBus;
using Mango.Services.OrderApi.Messages;
using Mango.Services.OrderApi.Models;
using Mango.Services.OrderApi.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.OrderApi.Messaging
{
    public class AzureServiceBusConsumer
    {
        private readonly OrderRepository _orderRepository;

        public AzureServiceBusConsumer(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        private async Task OnCheckoutMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;

            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkoutheaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderHeader = new()
            {
                UserId = checkoutheaderDto.UserId,
                FirstName = checkoutheaderDto.FirstName,
                LastName = checkoutheaderDto.LastName,
                OrderDetails = new List<OrderDetails>(),
                CardNumber = checkoutheaderDto.CardNumber,
                CouponCode = checkoutheaderDto.CouponCode,
                CVV = checkoutheaderDto.CVV,
                DiscountTotal = checkoutheaderDto.DiscountTotal,
                Email = checkoutheaderDto.Email,
                ExpiryMonthYear = checkoutheaderDto.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                OrderTotal = checkoutheaderDto.OrderTotal,
                PaymentStatus = false,
                Phone = checkoutheaderDto.Phone,
                PickupDateTime = checkoutheaderDto.PickupDateTime
            };
            foreach(var detailList in checkoutheaderDto.CartDetails)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = detailList.ProductId,
                    ProductName = detailList.Product.Name,
                    Price = detailList.Product.Price,
                    Count = detailList.Count
                };
                orderHeader.CartTotalItems += detailList.Count;
                orderHeader.OrderDetails.Add(orderDetails);
            }

            await _orderRepository.AddOrder(orderHeader);
        }
    }
}

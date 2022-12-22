using Mango.Services.OrderApi.Messages;
using Mango.Services.OrderApi.Models;
using Mango.Services.OrderApi.Repository;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mango.Services.OrderApi.Messaging
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly OrderRepository _orderRepository;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQConsumer(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                CheckoutHeaderDto checkoutheaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(content);
                HandleMessage(checkoutheaderDto).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume("checkoutqueue", false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(CheckoutHeaderDto checkoutheaderDto)
        {
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
            foreach (var detailList in checkoutheaderDto.CartDetails)
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

            PaymentRequestMessage paymentRequestMessage = new()
            {
                Name = orderHeader.FirstName + " " + orderHeader.LastName,
                CardNumber = orderHeader.CardNumber,
                CVV = orderHeader.CVV,
                ExpiryMonthYear = orderHeader.ExpiryMonthYear,
                OrderId = orderHeader.OrderHeaderId,
                OrderTotal = orderHeader.OrderTotal,
                Email = orderHeader.Email
            };

            try
            {
                //await _messageBus.PublishMessage(paymentRequestMessage, orderPaymentProcessTopic);
                //await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

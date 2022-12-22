using Mango.MessageBus;

namespace Mango.Services.PaymentApi.RabbitMQSender
{
    public interface IRabbitMQPaymentMessageSender
    {
        void SendMessage(BaseMessage baseMessage);
    }
}

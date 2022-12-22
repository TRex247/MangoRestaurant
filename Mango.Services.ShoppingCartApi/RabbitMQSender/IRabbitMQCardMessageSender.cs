using Mango.MessageBus;

namespace Mango.Services.ShoppingCartApi.RabbitMQSender
{
    public interface IRabbitMQCardMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName)
    }
}

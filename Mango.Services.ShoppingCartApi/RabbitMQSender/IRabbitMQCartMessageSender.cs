using Mango.MessageBus;

namespace Mango.Services.ShoppingCartApi.RabbitMQSender
{
    public interface IRabbitMQCartMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}

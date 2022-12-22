using Mango.MessageBus;

namespace Mango.Services.OrderApi.RabbitMQSender
{
    public interface IRabbitMQOrderMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}

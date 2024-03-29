﻿using Mango.MessageBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.Services.PaymentApi.RabbitMQSender
{
    public class RabbitMQPaymentMessageSender : IRabbitMQPaymentMessageSender
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;
        private const string ExchangeName = "DirectPaymentUpdate_Exchange";
        private const string PaymentEmailUpdateQueueName = "PaymentEmailUpdateQueueName";
        private const string PaymentOrderUpdateQueuename = "PaymentOrderUpdateQueuename";

        public RabbitMQPaymentMessageSender()
        {
            _hostname = "localhost";
            _password = "guest";
            _username = "guest";
        }
        public void SendMessage(BaseMessage message)
        {
            if(ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: false);
                channel.QueueDeclare(PaymentOrderUpdateQueuename, false, false, false, null);
                channel.QueueDeclare(PaymentEmailUpdateQueueName, false, false, false, null);

                channel.QueueBind(PaymentEmailUpdateQueueName, ExchangeName, "PaymentEmail");
                channel.QueueBind(PaymentOrderUpdateQueuename, ExchangeName, "PaymentOrder");

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: ExchangeName, "PaymentEmail", basicProperties: null, body:body);
                channel.BasicPublish(exchange: ExchangeName, "PaymentOrder", basicProperties: null, body: body);
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                //Log exception
            }
        }

        private bool ConnectionExists()
        {
            if(_connection != null)
            {
                return true;
            }
            CreateConnection();
            return _connection != null;
        }
    }
}

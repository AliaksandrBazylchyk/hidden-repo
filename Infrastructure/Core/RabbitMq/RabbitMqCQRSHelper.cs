using RabbitMQ.Client;
using System.Text;

namespace Core.RabbitMq
{
    public class RabbitMqCQRSHelper : IRabbitMqCQRSHelper
    {
        protected ConnectionFactory connectionFactory = new ConnectionFactory { HostName = "rabbitmq", UserName = "guest", Password = "guest"};

        public Task SendCreateMessageAsync(string message)
        {
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: QueuesHelpers.CREATEQUEUE, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: QueuesHelpers.EXCHANGE, routingKey: QueuesHelpers.CREATEQUEUE, basicProperties: null, body: body);

            return Task.CompletedTask;
        }

        public Task SendDeleteMessageAsync(string message)
        {
            throw new NotImplementedException();
        }
    }
}

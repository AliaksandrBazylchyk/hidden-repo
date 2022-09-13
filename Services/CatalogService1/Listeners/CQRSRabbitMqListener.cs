using Core.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CatalogService.Listeners
{
    public class CQRSRabbitMqListener : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private ILogger<CQRSRabbitMqListener> _logger { get; set; }

        public CQRSRabbitMqListener(
            ILogger<CQRSRabbitMqListener> logger
            )
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq", UserName = "guest", Password = "guest" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: QueuesHelpers.CREATEQUEUE, durable: false, exclusive: false, autoDelete: false, arguments: null);

            _logger = logger;

            _logger.LogInformation("Create queue consumption initialized.");                       
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());


                _logger.LogInformation($"Message consumed: {content}");

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(QueuesHelpers.CREATEQUEUE, false, consumer);

            return Task.CompletedTask;
        }
    }
}

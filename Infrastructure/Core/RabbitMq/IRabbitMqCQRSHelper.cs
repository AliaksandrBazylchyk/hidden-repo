namespace Core.RabbitMq
{
    public interface IRabbitMqCQRSHelper
    {
        Task SendCreateMessageAsync(string message);
        Task SendDeleteMessageAsync(string message);
    }
}

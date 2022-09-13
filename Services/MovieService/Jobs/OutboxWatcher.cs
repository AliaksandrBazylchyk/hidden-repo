using Core.Outbox;
using Core.RabbitMq;
using Enums;
using Microsoft.AspNetCore.Mvc;
using Models.OutboxEntities;
using Newtonsoft.Json;
using Quartz;

namespace MovieService.Jobs
{
    public class MovieOutboxWatchJob : IJob
    {
        private readonly ILogger<MovieOutboxWatchJob> _logger;
        private readonly IOutboxStore<MovieOutbox> _outboxStore;
        private readonly IRabbitMqCQRSHelper _rabbitMqCQRSHelper;
        public MovieOutboxWatchJob(
            ILogger<MovieOutboxWatchJob> logger,
            IOutboxStore<MovieOutbox> outboxStore,
            IRabbitMqCQRSHelper rabbitMqCQRSHelper
            )
        {
            _logger = logger;
            _outboxStore = outboxStore;
            _rabbitMqCQRSHelper = rabbitMqCQRSHelper;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var record = await _outboxStore.GetFirstRecordAsync();
            if (record is not null)
            {
                switch (record.Method)
                {
                    case MethodType.CREATE:
                        var result = _rabbitMqCQRSHelper.SendCreateMessageAsync(record.Object);
                        if (result.IsCompletedSuccessfully)
                        {
                            await _outboxStore.CommitRecordAsync(record.Id);
                            _logger.LogInformation("Object sended for creating: ", record.Object);
                            await _outboxStore.CommitRecordAsync(record.Id);
                        }
                        else
                        {
                            _logger.LogError("Object was founded but not sended. Check RabbitMQ health status. Object: ", JsonConvert.SerializeObject(record).ToString());
                        }                            
                        break;
                    case MethodType.DELETE:
                        break;
                    default:
                        _logger.LogWarning("Object with worng method type was finded at Job exection", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"));
                        break;
                }
            }
            else _logger.LogInformation("Job completed without sending at ", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"));
        }
    }
}

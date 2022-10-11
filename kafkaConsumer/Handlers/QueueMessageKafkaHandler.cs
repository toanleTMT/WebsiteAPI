
using Domains.Kafkas.EventDatas;
using Domains.Kafkas.EventDatas.Consumers;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Services.Kafka.QueueMessageService;

namespace kafkaConsumer.Handlers
{
    public class QueueMessageKafkaHandler : IMessageHandler<MessageEventData<SendMessageDto>>, IMessageKafkaHandler
    {
        private readonly ILogger _logger;
        private readonly IQueueMessageService _sendMessageService;

        public QueueMessageKafkaHandler(ILogger<QueueMessageKafkaHandler> logger, 
            IQueueMessageService sendMessageService)
        {
            _logger = logger;
            _sendMessageService = sendMessageService;
        }

        public Task Handle(IMessageContext context, MessageEventData<SendMessageDto> message)
        {
            Console.WriteLine(
                "Partition: {0} | Offset: {1} | Host: {2} | EventName: {3}",
                context.ConsumerContext.Partition,
                context.ConsumerContext.Offset,

                message.Data.Host,
                message.EventName);


            HandleAsync(context, message).Wait();

            return Task.CompletedTask;
        }

        public async Task HandleAsync(IMessageContext context, MessageEventData<SendMessageDto> message)
        {
            try
            {
                await _sendMessageService.GetMessage(message.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, message);
                Console.WriteLine($"HandleAsync: {ex.Message}");
            }
        }
    }
}

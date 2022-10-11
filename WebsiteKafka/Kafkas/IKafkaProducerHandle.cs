using Domains.Kafkas.EventDatas.Consumers;

namespace WebsiteKafka.Kafkas
{
    public interface IKafkaProducerHandle
    {
        Task ProducerSendMessage(SendMessageDto model);
    }
}

using Applications.Kafkas.Configs;
using Applications.Kafkas.EventBus;
using Confluent.Kafka;
using Domains.Kafkas.EventDatas;
using Domains.Kafkas.EventDatas.Consumers;
using Newtonsoft.Json;
using System.Text;

namespace WebsiteKafka.Kafkas.Messages
{
    public class SendMessageKafkaProducer : KafkaProducer, IKafkaProducerHandle
    {
        public SendMessageKafkaProducer(KafkaConfig configs) : base(configs)
        {
        }

        public async Task ProducerSendMessage(SendMessageDto model)
        {
            try
            {
                var mesageData = new MessageEventData<SendMessageDto>
                {
                    EventName = MessageEventData._SEND_MESSAGE,
                    Data = model,
                    Host = "localhost"
                };

                var messageKey = $"{model.Host}:{MessageEventData._SEND_MESSAGE}";
                var fullName = mesageData.GetType().FullName;
                var assemblyName = mesageData.GetType().Assembly.GetName().Name;

                var headers = new Headers();
                headers.Add("MessageType", Encoding.ASCII.GetBytes($"{fullName}, {assemblyName}"));

                var dr = await PRODUCER.ProduceAsync(TOPIC_NAME, new Message<string, string> { Key = messageKey, Value = JsonConvert.SerializeObject(mesageData), Headers = headers });
                Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<string, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }

        }
    }
}

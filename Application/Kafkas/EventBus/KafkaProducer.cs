using Applications.Kafkas.Configs;
using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Kafkas.EventBus
{
    public class KafkaProducer : IKafkaProducer
    {
        protected string TOPIC_NAME { get; private set; }

        protected IProducer<string, string> PRODUCER;

        public KafkaProducer(KafkaConfig configs)
        {
            TOPIC_NAME = configs.TopicName;

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configs.BootstrapServers,
                ClientId = Environment.MachineName,
                MessageTimeoutMs = configs.MessageTimeoutMs,
                RequestTimeoutMs = configs.RequestTimeoutMs,
                MessageMaxBytes = 100 * 1000000
            };

            PRODUCER = new ProducerBuilder<string, string>(producerConfig).Build();
        }

    }
}

using Domains.Kafkas.EventDatas.Consumers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Kafka.QueueMessageService
{
    public class QueueMessageService : IQueueMessageService
    {
        public QueueMessageService()
        {
        }

        public Task GetMessage(SendMessageDto data)
        {
            throw new NotImplementedException();
        }
    }
}

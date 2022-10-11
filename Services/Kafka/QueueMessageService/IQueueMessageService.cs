using Domains.Kafkas.EventDatas.Consumers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Kafka.QueueMessageService
{
    public interface IQueueMessageService
    {
        Task GetMessage(SendMessageDto data);
    }
}

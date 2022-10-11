using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Kafkas.EventDatas.Consumers
{
    public class SendMessageDto : IMessageEventInnerData
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Host { get; set; }

        public string Topic { get; set; }
    }
}

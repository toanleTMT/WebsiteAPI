using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Kafkas.EventDatas
{
    public interface IMessageEventInnerData
    {
    }

    public class MessageEventData
    {
        public string EventName { get; set; }

        public const string _SEND_MESSAGE = "_SEND_MESSAGE";
    }

    public class MessageEventData<T> : MessageEventData where T : IMessageEventInnerData
    {
        public string Host { get; set; }

        public T Data { get; set; }
    }

   
}

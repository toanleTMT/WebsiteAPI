using KafkaFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Kafkas.CustomJsons
{
    public class CustomMessageTypeResolver : IMessageTypeResolver
    {
        private const string MessageType = "MessageType";

        public Type OnConsume(IMessageContext context)
        {
            var typeName = context.Headers.GetString(MessageType);
            return Type.GetType(typeName);
        }

        public void OnProduce(IMessageContext context)
        {
            context.Headers.SetString(
                MessageType,
                $"{context.Message.GetType().FullName}, {context.Message.GetType().Assembly.GetName().Name}");
        }
    }
}

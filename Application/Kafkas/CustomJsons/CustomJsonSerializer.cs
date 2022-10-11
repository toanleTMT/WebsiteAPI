using KafkaFlow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Kafkas.CustomJsons
{
    public class CustomJsonSerializer : ISerializer
    {

        private const int DefaultBufferSize = 1024;

        private static readonly UTF8Encoding UTF8NoBom = new(false);
        private readonly JsonSerializerSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomJsonSerializer"/> class.
        /// </summary>
        /// <param name="settings">Json serializer settings</param>
        public CustomJsonSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomJsonSerializer"/> class.
        /// </summary>
        public CustomJsonSerializer()
            : this(new JsonSerializerSettings())
        {
        }

        /// <inheritdoc/>
        public Task SerializeAsync(object message, Stream output, ISerializerContext context)
        {
            using var sw = new StreamWriter(output, UTF8NoBom, DefaultBufferSize, true);
            var serializer = JsonSerializer.CreateDefault(this.settings);

            serializer.Serialize(sw, message);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<object> DeserializeAsync(Stream input, Type type, ISerializerContext context)
        {
            var serializer = JsonSerializer.CreateDefault(this.settings);

            using var sr = new StreamReader(input, UTF8NoBom, true, DefaultBufferSize, true);
            using var jsonReader = new JsonTextReader(sr);

            return Task.FromResult(serializer.Deserialize(jsonReader, type));
        }
    }
}

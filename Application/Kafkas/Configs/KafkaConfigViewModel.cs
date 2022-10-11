using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Kafkas.Configs
{
    public class KafkaConfigViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string BootstrapServers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RequestTimeoutMs { get; set; } = 30000;

        /// <summary>
        /// 
        /// </summary>
        public int MessageTimeoutMs { get; set; } = 30000;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Applications.Kafkas.Configs.KafkaConfig;

namespace Applications.Kafkas.Configs
{
    public interface IKafkaConfig
    {
    }
 
    public class KafkaConfig : KafkaConfigViewModel, IKafkaConfig 
    { 
    }
}

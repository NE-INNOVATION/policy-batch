using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace batch_policy
{
    public class KafkaService
    {
        public static async Task<string> SendMessage(string policy, string bootstrapUrl = "policy-cluster-kafka-bootstrap:9092")
        {
            Console.WriteLine($"Begin Publish to Kafka");
            var config = new ProducerConfig {
                BootstrapServers = bootstrapUrl, 
            };

            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var dr = await p.ProduceAsync("policy", new Message<Null, string> { Value=policy });
                    Console.WriteLine($"Delivered to topic '{dr.Topic}'");
                    return $"Delivered to topic '{dr.Topic}'";
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
            return "success";
        }
    }
}

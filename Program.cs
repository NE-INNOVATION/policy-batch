using System;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace batch_policy
{
    class Program
    {
        static string url = Environment.GetEnvironmentVariable("POLICY-URL") ?? "http://istio-ingressgateway-istiosystem.apps.openshift.ne-innovation.com/policy/message";
        static string bootstrap = Environment.GetEnvironmentVariable("BOOTSTRAP-SERVER") ?? "my-cluster-kafka-bootstrap:9092";
        
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Job Triggered!");
                Console.WriteLine("Argument lenght and value = " + args?.Length);
                long timeToPoll = 0;

                if (!(args.Length > 0 && long.TryParse(args[0], out timeToPoll)))
                {
                    timeToPoll = 10000;
                }

                var client = new HttpClient();
                var result = client.GetAsync( url + "?time=" + timeToPoll).Result;
                Console.WriteLine($"received message from kafka {result}");
                
                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    if (response != null && response != "[]") 
                     KafkaService.SendMessage(response,bootstrap);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}

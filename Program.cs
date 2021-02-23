using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Linq;

namespace batch_policy
{
    class Program
    {
        static string url = Environment.GetEnvironmentVariable("POLICY-URL") ?? "http://istio-ingressgateway-istio-system.apps.awsopenshift.ne-innovation.com/policy/message";
        static string bootstrap = Environment.GetEnvironmentVariable("BOOTSTRAP-SERVER") ?? "my-cluster-kafka-bootstrap:9092";

        static string sleepTime = Environment.GetEnvironmentVariable("POLL_TIME") ?? "1000";

        static void Main(string[] args)
        {
            Console.WriteLine("Job Triggered!");


            while (true)
            {
                try
                {

                    //Console.WriteLine($"Argument lenght and value ={args.FirstOrDefault()} ");
                    int timeToPoll = 0;
                    

                    if (!(args.Length > 0 && int.TryParse(args[0], out timeToPoll)))
                    {
                        timeToPoll = 1000;
                    }

                    Thread.Sleep(timeToPoll);

                    var client = new HttpClient();
                    var result = client.GetAsync(url + "?time=" + timeToPoll).Result;
                    //Console.WriteLine($"received message from kafka {result}");

                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        if (response != null && response != "")
                        {
                            Console.WriteLine($"Content is not empty {response}");
                            KafkaService.SendMessage(response, bootstrap);
                        }
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }
    }
}

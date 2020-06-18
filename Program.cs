using System;
using System.IO;
using System.Threading;

namespace batch_policy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Job Triggered!");

            Thread.Sleep(20000);

            if(!File.Exists("./test.txt")) File.Create("./test.txt");
            
            Console.WriteLine("Completed");


        }
    }
}

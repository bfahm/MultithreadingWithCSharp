using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsyncTeaMaker
{
    class ProgramSync
    {
        static void _Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            MakeTea();
            stopwatch.Stop();
            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Time Elapsed: {stopwatch.Elapsed.TotalMilliseconds/1000} seconds");
        }

        static string BoilWater()
        {
            Console.WriteLine("Start the kettle");
            Console.WriteLine("Waiting for the kettle");
            Task.Delay(3000).Wait();

            Console.WriteLine("Kettle Finished Boiling");

            return "Hot water";
        }

        static string PrepareCups(int numberOfCups)
        {
            for (int i = 0; i < numberOfCups; i++)
            {
                Console.WriteLine($"Taking cup #{i + 1} out.");
                Console.WriteLine("Putting tea and sugar in the cup");
                Task.Delay(3000).Wait();
            }

            Console.WriteLine("Finished preparing the cups");

            return "cups";
        }

        static string WarmupMilk()
        {
            Console.WriteLine("Pouring milk into a container");
            Console.WriteLine("Putting the container in microwave");
            Console.WriteLine("Warming up the milk");
            Task.Delay(5000).Wait();

            Console.WriteLine("Finished warming up the milk");

            return "Warm Milk";
        }

        static void MakeTea()
        {
            var water = BoilWater();
            var cups = PrepareCups(2);
            Console.WriteLine($"Pouring {water} into {cups}");

            cups = "cups with tea";

            var warmMilk = WarmupMilk();
            Console.WriteLine($"Adding {warmMilk} into {cups}");
        }
    }
}

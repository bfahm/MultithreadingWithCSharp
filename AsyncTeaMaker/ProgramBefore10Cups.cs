using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncTeaMaker
{
    class ProgramBefore10Cups
    {
        static async Task _Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await MakeTeaAsync();
            stopwatch.Stop();
            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Time Elapsed: {stopwatch.Elapsed.TotalMilliseconds / 1000} seconds");
        }

        static async Task<string> BoilWaterAsync()
        {
            Console.WriteLine("Start the kettle");
            Console.WriteLine("Waiting for the kettle");
            await Task.Delay(3000);

            Console.WriteLine("Kettle Finished Boiling");

            return "Hot water";
        }

        static async Task<string> PrepareCupsAsync(int numberOfCups)
        {
            Task[] eachCupTask = Enumerable.Range(1, numberOfCups).Select(index => 
            {
                Console.WriteLine($"Taking cup #{index} out.");
                Console.WriteLine("Putting tea and sugar in the cup");
                return Task.Delay(3000);
            }).ToArray();

            await Task.WhenAll(eachCupTask);

            Console.WriteLine("Finished preparing the cups");

            return "cups";
        }

        static async Task<string> WarmupMilkAsync()
        {
            Console.WriteLine("Pouring milk into a container");
            Console.WriteLine("Putting the container in microwave");
            Console.WriteLine("Warming up the milk");
            await Task.Delay(5000);

            Console.WriteLine("Finished warming up the milk");

            return "Warm Milk";
        }

        //static async Task MakeTeaAsync()
        //{
        //    var water = await BoilWaterAsync();
        //    var cups = await PrepareCupsAsync(2);
        //    Console.WriteLine($"Pouring {water} into {cups}");

        //    cups = "cups with tea";

        //    var warmMilk = await WarmupMilkAsync();
        //    Console.WriteLine($"Adding {warmMilk} into {cups}");
        //}

        static async Task MakeTeaAsync()
        {
            var waterBoilingTask = BoilWaterAsync();
            var preparingCupsTask = PrepareCupsAsync(2);
            var warmingMilkTask = WarmupMilkAsync();
            
            var cups = await preparingCupsTask;
            var water = await waterBoilingTask;
            
            Console.WriteLine($"Pouring {water} into {cups}");
            cups = "cups with tea";
            
            var warmMilk = await warmingMilkTask;
            Console.WriteLine($"Adding {warmMilk} into {cups}");
        }
    }
}

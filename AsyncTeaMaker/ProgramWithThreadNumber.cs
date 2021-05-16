using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTeaMaker
{
    class ProgramWithThreadNumber
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
            WriteLineWithCurrentThreadId("Start the kettle");
            WriteLineWithCurrentThreadId("Waiting for the kettle");
            await Task.Delay(3000);

            WriteLineWithCurrentThreadId("Kettle Finished Boiling");

            return "Hot water";
        }

        static async Task<string> PrepareCupsAsync(int numberOfCups)
        {
            Task[] eachCupTask = Enumerable.Range(1, numberOfCups).Select(index =>
            {
                WriteLineWithCurrentThreadId($"Taking cup #{index} out.");
                WriteLineWithCurrentThreadId("Putting tea and sugar in the cup");
                return Task.Delay(3000);
            }).ToArray();

            await Task.WhenAll(eachCupTask);

            WriteLineWithCurrentThreadId("Finished preparing the cups");

            return "cups";
        }

        static async Task<string> WarmupMilkAsync()
        {
            WriteLineWithCurrentThreadId("Pouring milk into a container");
            WriteLineWithCurrentThreadId("Putting the container in microwave");
            WriteLineWithCurrentThreadId("Warming up the milk");
            await Task.Delay(5000);

            WriteLineWithCurrentThreadId("Finished warming up the milk");

            return "Warm Milk";
        }

        static async Task MakeTeaAsync()
        {
            var waterBoilingTask = BoilWaterAsync();
            var preparingCupsTask = PrepareCupsAsync(2);
            var warmingMilkTask = WarmupMilkAsync();
            
            var cups = await preparingCupsTask;
            var water = await waterBoilingTask;
            
            WriteLineWithCurrentThreadId($"Pouring {water} into {cups}");
            cups = "cups with tea";
            
            var warmMilk = await warmingMilkTask;
            WriteLineWithCurrentThreadId($"Adding {warmMilk} into {cups}");
        }

        public static void WriteLineWithCurrentThreadId(string textToPrint) 
            => Console.WriteLine($"Thread #{Thread.CurrentThread.ManagedThreadId} | {textToPrint}");
    }
}

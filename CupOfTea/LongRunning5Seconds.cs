using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CupOfTea
{
    static class LongRunning5Seconds
    {
        // This can be used for asynchronous utilization
        private static Task LongRunning5SecondsAsync()
        {
            ("Long Running 5 Seconds has started, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            var task = Task.Delay(5000);
            ("Long Running 5 Seconds should be ticking right now, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            return task;
        }

        // But this can't..
        private static Task BadLongRunning5SecondsSync()
        {
            ("Long Running 5 Seconds has started, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            Task.Delay(5000).Wait();
            ("Long Running 5 Seconds has certainly finished, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            return Task.CompletedTask;
        }

        public static async Task BadRunTwo5SecondsSimultainously()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ("Parent method has started child task, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            var task = BadLongRunning5SecondsSync();

            ("Parent method is waiting for its task, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            await Task.Delay(5000);
            ("Parent method has finished, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();

            ("Parent method has started waiting to child 5 seconds, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            await task; // Make sure the first task has finished before exiting the function

            ("Long Running 5 Seconds has finished, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();

            stopWatch.Stop();
            $"Parent method has finsihed, elapsed seconds =  {stopWatch.ElapsedMilliseconds / 1000}".Dump();
        }

        public static async Task BadRunTwo5SecondsSimultainouslyFix()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ("Parent method has started child task, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();

            var task = Task.Run(() =>
            {
                BadLongRunning5SecondsSync();
            });

            ("Parent method is waiting for its task, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            await Task.Delay(5000);
            ("Parent method has finished, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();

            ("Parent method has started waiting to child 5 seconds, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            await task; // Make sure the first task has finished before exiting the function

            ("Long Running 5 Seconds has finished, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();

            stopWatch.Stop();
            $"Parent method has finsihed, elapsed seconds =  {stopWatch.ElapsedMilliseconds / 1000}".Dump();
        }

        public static async Task RunTwo5SecondsSynchronously()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ("Parent method started waiting for child task, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            await LongRunning5SecondsAsync();
            ("Parent method finished waiting for child task, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();

            ("Parent method is waiting for its task, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            await Task.Delay(5000);
            ("Parent method has its task finished, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();

            stopWatch.Stop();
            $"Parent method has finsihed, elapsed seconds =  {stopWatch.ElapsedMilliseconds / 1000}".Dump();
        }

        public static async Task RunTwo5SecondsSimultainously()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ("Parent method has started child task, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            var task = LongRunning5SecondsAsync();

            ("Parent method is waiting for its task, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            await Task.Delay(5000);
            ("Parent method has finished, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();

            ("Parent method has started waiting to child 5 seconds, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();
            await task; // Make sure the first task has finished before exiting the function

            ("Long Running 5 Seconds has finished, thread ID: " + Thread.CurrentThread.ManagedThreadId).Dump();

            stopWatch.Stop();
            $"Parent method has finsihed, elapsed seconds =  {stopWatch.ElapsedMilliseconds / 1000}".Dump();
        }
    }
}

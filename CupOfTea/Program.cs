using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CupOfTea
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://www.youtube.com/watch?v=il9gl8MH17s&t=1s
            //https://stackoverflow.com/questions/12144077/async-await-when-to-return-a-task-vs-void

            LongRunning5Seconds.RunTwo5SecondsSimultainously().Wait();
        }

        
    }
}

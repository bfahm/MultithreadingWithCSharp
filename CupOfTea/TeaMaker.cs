using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CupOfTea
{
    class TeaMaker
    {
        #region Sync
        string BoilWater()
        {
            "Start the kettle".Dump();
            "Waiting for the kettle".Dump();

            Task.Delay(3000).Wait();

            "Kettle Finished Boiling".Dump();
            
            return "Hot water";
        }

        public Task<string> BoilWaterTask()
        {
            return Task.FromResult(BoilWater());
        }

        public void MakeTea()
        {
            var water = BoilWater();

            "Take the cups out".Dump();
            "Put tea in cups".Dump();

            $"Pour {water} into cups".Dump();
        }
        #endregion

        // This is a basic async method in C#
        public async Task<string> BoilWaterAsync()
        {
            // The method is executed just like any other method, synchronoulsy
            "Start the kettle".Dump();
            "Waiting for the kettle".Dump();

            // ... until it hits the "await keyword".
            // The "await" keyword is where things can get asynchronous.
            // An "await" keyword expects an "awaitable", then it examines whether the awaitable has already completed.
            // If “await” sees that the awaitable has not completed, then it acts asynchronously,
            // waits for the "awaitable" to continue till it completes,
            await Task.Delay(3000);
            // then the method just continues running (synchronously, just like a regular method).

            // Two points to note down:
            // 1. The awaitable **might** have already started before actually starting to await it.
            // 2. After the awaitable is completed, the remainder of the async function (the calling one)
            //    will execute on a "context" that was captured before the "await" returned.
            //    Note the context switching overhead that was caused by the await keyword.
            /*
             * So, what is a "context":
             *  1. If you’re on a UI thread, then it’s a UI context.
             *  2. If you’re responding to an ASP.NET request, then it’s an ASP.NET request context.
             *  3. Otherwise, it’s usually a thread pool context.
             *  
             *  .. see example from Stephen Cleary.
             */

            /**
             * "I like to think of “await” as an “asynchronous wait”. That is to say, the async method pauses until 
             * the awaitable is complete (so it waits), but the actual thread is not blocked (so it’s asynchronous)."
             *          -- Stephen Cleary https://blog.stephencleary.com/2012/02/async-and-await.html
             */

            "Kettle Finished Boiling".Dump();

            return "Hot water";
        }

        /* Quoted from Stephen Cleary's blog post https://blog.stephencleary.com/2012/02/async-and-await.html
         * Unwrap comment folds
        // WinForms example (it works exactly the same for WPF).
        private async void DownloadFileButton_Click(object sender, EventArgs e)
        {
            // Since we asynchronously wait, the UI thread is not blocked by the file download.
            await DownloadFileAsync(fileNameTextBox.Text);

            // Since we resume on the UI context, we can directly access UI elements.
            resultTextBox.Text = "File downloaded!";
        }

        // ASP.NET example
        protected async void MyButton_Click(object sender, EventArgs e)
        {
            // Since we asynchronously wait, the ASP.NET thread is not blocked by the file download.
            // This allows the thread to handle other requests while we're waiting.
            await DownloadFileAsync(...);

            // Since we resume on the ASP.NET context, we can access the current request.
            // We may actually be on another *thread*, but we have the same ASP.NET request context.
            Response.Write("File downloaded!");
        }
        */

        public async Task MakeTeaAsync()
        {
            var boilingWaterTask = BoilWaterAsync();

            "Take the cups out".Dump();
            "Put tea in cups".Dump();

            var water = await boilingWaterTask;

            $"Pour {water} into cups".Dump();
        }

        public void NotaBenne_1()
        {
            /* Calling a function returning a task results in the creation of a 
             * "hot" task, a task that runs immediately the point it gets called.
             * 
             * Note: Calling the function or instantiating a new variable of type Task using the function 
             * gives the same result.
             * 
             * SEE NotaBenne_2..
            */

            // BoilWaterAsync();
            // is the same as
            var myTask = BoilWaterAsync();

            /* In case the function is async:
             * Both calls causes the start of the task, without waiting (or 'awaiting') any awaitable child tasks,
             * execution of the function will continue once the long running child task finishes, but it is not 
             * guaranteed the the rest of the function would be reached if not awaited.
             */
            
            /* In case the function is NOT async and there are no inner awaited calls:
             * Both calls will causes the start of the task, waiting for it to end.
             */
        }

        public void NotaBenne_2()
        {
            // HOT vs COLD tasks..

            // var boilingWater_HotTask = BoilWaterAsync(); // This is a hot task, it starts off immediately

            // This, on the other hand, is a cold task, containing a code similar to the function BoilWaterAsync()
            var boilingWater_ColdTask = new Task(() =>
            {
                "Start the kettle".Dump();
                "Waiting for the kettle".Dump();

                Task.Delay(3000).Wait();

                "Kettle Finished Boiling".Dump();

                "Throwing hot water in sink..".Dump();
            });

            // The above task will not start until calling a function that does so:
            boilingWater_ColdTask.Start();
            boilingWater_ColdTask.Wait();
        }

        public void NotaBenne_3()
        {
            /* Always make sure that the awaitable task is awaited to the top-level caller, this means that
             * if an async function has an awaited task, but the function itself is not awaited, the code execution
             * will continue without waiting the child awaited task.
            */

            // Note that the current function is not asyncronous, and has a return type void.

            MakeTeaAsync(); // Will not wait for boilingWaterTask to finish
            // but this call:
            // await MakeTeaAsync(); will wait for the water to boil,
            // but will throw a compiler error since the current function is not marked as async..
            // You'll need to call MakeTeaAsync from an async function THAT IS AWAITED.

            // Await the function means you'll await all the child tasks created inside the function.
        }

        public Task<string> NotaBenne_4()
        {
            /* Okay, so let's say we are implementing an interface, one of it's methods requires a Task<string> 
             * to be returned, BUT the execution of the method itself does not require an asynchronous setup,
             * it would not block the main thread and can run synchronously without problems.
            */

            // This will cause a compiler error because it expects a Task<string> not just a string
            // return "My Result";

            /* DONT:
             * Don't convert the method to be async, this will add a needless state machine in the background 
             * for the execution of the method asynchronously, which forms added logic and complexity.
             */

            // DO:
            return Task.FromResult("My Result");
            // or
            //return Task.CompletedTask; // if it requires a return type Task not Task<string>
        }

        public Task<string> NotaBenne_5()
        {
            // Now we have an HTTP client retrieving some text from an external website.

            var client = new HttpClient();
            var content = client.GetStringAsync("my.website.com");

            /* There would be two scenarios here, first one:
             1. You just want to return the string (or the task of retrieving the string) without any type 
                of pre-processing or validtion:
                DO:
                • Return the task directly without awaiting it, and converting the function to an async one.
                • This will skip the creation of a needless state machine and will just postpone processing the task till it's
                  finally needed.
                DONT:
                • Don't await GetStringAsync call.

             2. You need to process the result, or validate it before returning the result
                DO: await the call, now the state machine is needed and delegating the process will not help.
            */

            return content;
        }

        public void NotaBenne_6()
        {
            // Assume the current function is a top-level one, and we need to call a method that is returning a task

            var task = NotaBenne_5();

            // DONT
            var result = task.Result;

            // DONT
            task.Wait();

            // DONT
            task.GetAwaiter().GetResult();

            /** The above three DONTs will cause the application's main thread to be blocked till the task is finished,
             *  it might stop responding or processing new requests till the task is awaited.
            */

            // DO
            /* Embrace the asynchronous nature of the task, await it and return the result. Tasks being probagated through
             * out your code is normal, just try to delay doing so as much as possible.
             */
        }

        public void NotaBenne_7()
        {
            // Both tasks has started running simultainously.
            var longRunningAsyncTask1 = BoilWaterAsync();
            var longRunningAsyncTask2 = BoilWaterAsync();

            // This will wait for both tasks to finish while blocking the main thread waiting for results;
            // Notice the return type of WaitAll, "void"
            Task.WaitAll(longRunningAsyncTask1, longRunningAsyncTask2);

            // SEE NotaBenne_8
        }

        public Task NotaBenne_8()
        {
            // SEE NotaBenne_7

            // Both tasks has started running simultainously.
            var longRunningAsyncTask1 = BoilWaterAsync();
            var longRunningAsyncTask2 = BoilWaterAsync();

            // This will return a task representing the result of both tasks.
            // It's upto us to retrieve the result in one of the following ways:
            // 1. await the result inside the method, hence the method must be declared async
            // 2. return the task as is and do either of the following
            //      • let a calling function "await" the result 
            //      • let a calling function run a task in a separate thread using Task.Run()
            
            // Notice the return type of WhenAll
            return Task.WhenAll(longRunningAsyncTask1, longRunningAsyncTask2);
        }

        public async void NotaBenne_9()
        {
            // Summarizing the key differences between these three calls.

            /**
             * Preliminary: BoilWaterTask() is a long running task presented as a function, 
             * it "WAITS" a period of time in a blocking manner.
             * 
             * The next call is not an asynchronous method call, it returns a task, 
             * but by the time it returns, the entire method would have been done anyway.
             * 
             * So, the following call is a wrong utilization of the method, and should be called within a "Task.Run()" call
             * or awaited correctly to prevent blocking the main thread.
             */

            // The following blocks the thread.
            Task<string> longRunningTask = BoilWaterTask();
            
            /** 
             * The following DOES NOT block thread.
             * In a special scenario, if the function BoilWaterTask() boils the water through a network (I/O) call somehow,
             * awaiting the call frees the thread, to be used for something else, after the water boils.
             * this thread (or another idle one from the thread pool) is used to continue running the function.
            */
            string longRunningResult = await BoilWaterTask();

            // Preliminary: BoilWaterAsync() is an async method, meaning that it "might" contain an awaited call
            // So, this will also return a Task<string> except that, any awaited calls inside the function "might" 
            // not have finished by the time the next line is called, since the function is not awaited.
            Task<string> longRunningAsyncMethod = BoilWaterAsync();

            // Essentially the same as awaiting BoilWaterTask() since both will cause the thread to wait
            // asynchronously till the 5 seconds passes.
            string longRunningAsyncResult = await BoilWaterAsync();

            /*
             * The key takeaway points here are:
             * • The "async" keyword enables the "await" keyword in to be used in a method and changes how method results 
             *   are handled. **That’s all the async keyword does!** It does not run this method on a different thread, 
             *   or do any other kind of hidden magic. 
             * 
             * • A function is awaitable because it returns a Task or a Task<T>, not because it is marked async, so we 
             *   can await a function that is not async.
             */

            // Remember, an async funtion can return either on of the three types: void, Task, or Task<T>
            /*
             * This leaves us to another note: You cannot await a function returning void, hence, 
             * you should always return a Task in async functions unless there is an
             * absolute reason not to do so (a caller to the function expects a void return type), or that the function
             * itself is a top level function and there is not way it will ever be needed to call it.
             */
        }
    }
}

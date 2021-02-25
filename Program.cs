using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            //模拟并发
            while (true)
            {
                Task.Run(Producer);
                 //Task.Run(Producer2);
                //Task.Run(ProducerQueue);
                Thread.Sleep(200);
            }

        }
        /// <summary>
        /// 如果处理程序耗时>请求耗时，也就是说引起并发，就会导致死锁
        /// </summary>
        static void Producer()
        {
            var result = Process().Result;
            //或者
            //Process().Wait();
        }
        /// <summary>
        /// 正常
        /// </summary>
        static void Producer2()
        {
            var result = Process().ConfigureAwait(false);
        }
        //cpu 内存均正常
        static async Task Producer3()
        {
            await Process();
        }

        /// <summary>
        /// 
        /// </summary>
        static void ProducerQueue()
        {
            ProcessQueue().Wait();
            //或者
            //Process().Wait();
        }
        /// <summary>
        /// 正常
        /// </summary>
        static void ProducerQueue2()
        {
            var result = ProcessQueue().ConfigureAwait(false);
        }
        //cpu 内存均正常
        static async Task ProducerQueue3()
        {
            await ProcessQueue();
        }

        static async Task<bool> Process()
        {
            ConsoleWrite("Start");
            await Task.Run(() =>
            {
                //模拟任务执行耗时
                Thread.Sleep(1000);
            });

            ConsoleWrite("End");
            return true;
        }

      

        static async Task ProcessQueue()
        {
            Console.WriteLine("Start - " + GetCurrentThreadID());

            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine("Hello, " + (string)state + GetCurrentThreadID());
            }, await GetName());
            Console.WriteLine("Ended - " + GetCurrentThreadID());
        }

        private static async Task<string> GetName()
        {
            Thread.Sleep(1000);
            return "ZhiXin" + GetCurrentThreadID();
        }
        private static string GetCurrentThreadID()
        {
            return $" {DateTime.Now.ToLongTimeString()} --ThreadId[{Thread.CurrentThread.ManagedThreadId.ToString("0000")}]";
        }
        private static void ConsoleWrite(string type)
        {
            if (type == "Start")
            {
                //Console.BackgroundColor = ConsoleColor.Blue; //设置背景色
                Console.ForegroundColor = ConsoleColor.Green; //设置前景色，即字体颜色
            }
            else
            {
               // Console.BackgroundColor = ConsoleColor.Red; //设置背景色
                Console.ForegroundColor = ConsoleColor.Red; //设置前景色，即字体颜色
            }
            Console.WriteLine($"{type} - " + GetCurrentThreadID());
        }
    }
}

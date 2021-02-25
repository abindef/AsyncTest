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
            for (int i = 0; i < 1000; i++)
            {
               
                Task.Run(() => 
                     Producer(i)
                );
                // Task.Run(Producer3);
                // Task.Run(ProducerQueue);
                Thread.Sleep(200);
            }

        }
        /// <summary>
        /// 如果处理程序耗时>请求耗时，也就是说引起并发，就会导致死锁
        /// </summary>
        static void Producer(int i)
        {
            var result = Process(i).Result;
            //或者
            //Process().Wait();
        }
        /// <summary>
        /// 正常
        /// </summary>
        static void Producer2(int i)
        {
            var result = Process(i).ConfigureAwait(false);
        }
        //cpu 内存均正常
        static async Task Producer3(int i)
        {
            await Process(i);
        }

        /// <summary>
        /// 
        /// </summary>
        static void ProducerQueue(int i)
        {
            ProcessQueue(i).Wait();
            //或者
            //Process().Wait();
        }
        /// <summary>
        /// 正常
        /// </summary>
        static void ProducerQueue2(int i)
        {
            var result = ProcessQueue(i).ConfigureAwait(false);
        }
        //cpu 内存均正常
        static async Task ProducerQueue3(int i)
        {
            await ProcessQueue(i);
        }

        static async Task<bool> Process(int i)
        {
            ConsoleWrite($"Start{i}");
            await Task.Run(() =>
            {
                //模拟任务执行耗时
                Thread.Sleep(1000);
            });

            ConsoleWrite($"End{i}");
            return true;
        }



        static async Task ProcessQueue(int i)
        {
            ConsoleWrite($"Start{i}");
            ThreadPool.QueueUserWorkItem(state =>
            {
                ConsoleWrite("Hello" + (string)state);
            }, await GetName(i));
            ConsoleWrite($"End{i}");
        }

        private static async Task<string> GetName(int i)
        {
            Thread.Sleep(1000);
            Random r = new Random();
            return $"ZhiXin[{r.Next(100, 999)}]-[{i}]";
        }
        private static string GetCurrentThreadID()
        {
            return $" {DateTime.Now.ToLongTimeString()} --ThreadId[{Thread.CurrentThread.ManagedThreadId.ToString("0000")}]";
        }
        private static void ConsoleWrite(string type)
        {
            if (type.Contains("Start"))
            {
                //Console.BackgroundColor = ConsoleColor.Blue; //设置背景色
                Console.ForegroundColor = ConsoleColor.Green; //设置前景色，即字体颜色
            }
            else if (type.Contains("End"))
            {
                // Console.BackgroundColor = ConsoleColor.Red; //设置背景色
                Console.ForegroundColor = ConsoleColor.Red; //设置前景色，即字体颜色
            }
            Console.WriteLine($"{type} - " + GetCurrentThreadID());
        }
    }
}

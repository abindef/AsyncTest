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
        static void Producer2()
        {
            var result = Process().ConfigureAwait(false);
        }
        //cpu 内存均正常
        static async Task Producer3()
        {
            await Process();
        }

        static async Task<bool> Process()
        {
            Console.WriteLine("Start - " + DateTime.Now.ToLongTimeString());
            await Task.Run(() =>
            {
                //模拟任务执行耗时
                Thread.Sleep(1000);
            });

            Console.WriteLine("Ended - " + DateTime.Now.ToLongTimeString());
            return true;
        }
    }
}

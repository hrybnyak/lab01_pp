using System;
using System.Threading;

namespace CasSynchronization
{
    class Program
    {
        private static int value = 0;
        //unsynchronized
        // static void Main(string[] args)
        // {
        //     var t1 = new Thread(() =>
        //     {
        //         for (int i = 0; i < 100; i++)
        //         {
        //             value++;
        //             Console.WriteLine(value);
        //         }
        //     });
        //     var t2 = new Thread(() =>
        //     {
        //         for (int i = 0; i < 100; i++)
        //         {
        //             value--;
        //             Console.WriteLine(value);
        //         }
        //     });
        //     t1.Start();
        //     t2.Start();
        //     t1.Join();
        //     t2.Join();
        // }
        //
        //synchronized
        static void Main(string[] args)
        {
            var t1 = new Thread(() =>
            {
                Monitor.Lock();
                for (int i = 0; i < 100; i++)
                {
                    value++;
                    Console.WriteLine(value);
                }
                Monitor.Unlock();
            });
            var t2 = new Thread(() =>
            {
                Monitor.Lock();
                for (int i = 0; i < 100; i++)
                {
                    value--;
                    Console.WriteLine(value);
                }
                Monitor.Unlock();
            });
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
        }
    }
}
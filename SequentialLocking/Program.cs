using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequentialLocking
{
    class Program
    {
        static object lock1 = new object();
        static object lock2 = new object();

        static void method1()
        {
            lock (lock1)
            {
                Console.WriteLine("Method 1 got lock 1");
                Console.WriteLine("Method 1 waiting for lock 2");
                lock (lock2)
                {
                    Console.WriteLine("Method 1 got lock 2");
                }
                Console.WriteLine("Method 1 released lock 2");
            }
            Console.WriteLine("Method 1 released lock 1");
        }

        static void method2()
        {
            lock (lock2)
            {
                Console.WriteLine("Method 2 got lock 2");
                Console.WriteLine("Method 2 waiting for lock 1");
                lock (lock1)
                {
                    Console.WriteLine("Method 2 got lock 1");
                }
                Console.WriteLine("Method 2 released lock 1");
            }
            Console.WriteLine("Method 2 released lock 2");
        }

        //this runs fine
        //static void Main(string[] args)
        //{
        //    method1();
        //    method2();
        //    Console.WriteLine("press any key to stop");
        //    Console.ReadKey();

        //}

        static void Main(string[] args)
        {
            Task t1 = Task.Run(() => method1());
            Task t2 = Task.Run(() => method2());
            //t2.Wait();
            Console.WriteLine("press any key to stop");
            Console.ReadKey();
        }
    }
}

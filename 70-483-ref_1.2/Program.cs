using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskInteraction
{
    class Program
    {
        static long sharedTotal;

        static int[] items = Enumerable.Range(0, 5000001).ToArray();
        //create a lockobject 
        static object sharedTotalLock = new object();

        static void addRangeOfValues(int start, int end)
        {
            long subtotal = 0;
            while (start < end)
            {
                subtotal = subtotal + items[start];
                start++;
            }

            //lock the method to make it atomic
            //lock (sharedTotalLock)
            //{
            //    sharedTotal = sharedTotal + subtotal;
            //}

            // or do the same with Monitor and make use of try catch
            //Monitor.Enter(sharedTotalLock);
            //try
            //{
            //    sharedTotal = sharedTotal + subtotal;
            //}
            //finally
            //{
            //    Monitor.Exit(sharedTotalLock);
            //}

            //There is an better/easier solution using Interlocked.class
            Interlocked.Add(ref sharedTotal, subtotal);
            
        }


        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>();

            int rangeSize = 1000;
            int rangeStart = 0;

            while (rangeStart < items.Length)
            {
                int rangeEnd = rangeStart + rangeSize;

                if (rangeEnd > items.Length)
                {
                    rangeEnd = items.Length;
                }

                //create local copies of the parameters
                int rs = rangeStart;
                int re = rangeEnd;

                tasks.Add(Task.Run(() => addRangeOfValues(rs, re)));
                rangeStart = rangeEnd;
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("The total is : {0}", sharedTotal);
            Console.ReadKey();
        }
    }
}

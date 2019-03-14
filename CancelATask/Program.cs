using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CancelATask
{
    class Program
    {
        //Shared between front and backendtask so task can be needly stopped
        static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();


        // note the difference between CancellationTokenSource and CancellationToken
        static void Clock(CancellationToken cancellation)
        {
            int tickcount = 0;
            while (!cancellationTokenSource.IsCancellationRequested  && tickcount < 10)
            {
                Console.WriteLine("Tick");
                Thread.Sleep(500);
                tickcount++;
            }
            if (cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine("cancelled throwing exception");
                cancellation.ThrowIfCancellationRequested();
            }
            
        }

        static void Main(string[] args)
        {
            Task.Run(() => Clock(new CancellationToken()));
            Console.WriteLine("Hit any key to stop the clock");
            Console.ReadKey();
            cancellationTokenSource.Cancel();
            Console.WriteLine("Clock Stopped");
            Console.ReadKey();
           
        }
    }
}

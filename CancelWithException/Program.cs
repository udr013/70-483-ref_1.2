using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CancelWithException
{
    class Program
    {
        // note the difference between CancellationTokenSource and CancellationToken
        static void Clock(CancellationToken cancellationToken)
        {
            int tickcount = 0;
            while (!cancellationToken.IsCancellationRequested && tickcount < 10)
            {
                Console.WriteLine("Tick");
                Thread.Sleep(500);
                tickcount++;
            }
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("cancelled throwing exception");
                cancellationToken.ThrowIfCancellationRequested();
            }

        }

        static void Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Task clock = Task.Run(() => Clock(cancellationTokenSource.Token));
            Console.WriteLine("Hit any key to stop the clock");
            Console.ReadKey();

            if (clock.IsCompleted)
            {
                Console.WriteLine("clock task completed");
            }
            else
            {
                try
                {
                    cancellationTokenSource.Cancel();
                    clock.Wait();
                }
                catch(AggregateException ex){
                    Console.WriteLine("Clock stopped: {0}", ex.InnerExceptions[0].ToString());
                }
            }
  
            Console.ReadKey();

        }
    }
}

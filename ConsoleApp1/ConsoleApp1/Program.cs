using System;
using AsyncAwaitConsoleTest;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // questo commento è stato editato in GitHub
            // questo commento è stato aggiunto in VS

            // Async Await feature
            var lr = new LongRunning();
        }

        static int Add()
        {
            var x1 = 1;
            var x2 = 2;
            var sum = x1 + x2;
            return sum;
        }
    }
}

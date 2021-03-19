using System;
using AsyncAwaitConsoleTest;
using TcpServer;

namespace ConsoleApp1
{
    class Program
    {
        async static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // questo commento è stato editato in GitHub
            // questo commento è stato aggiunto in VS

            // Async Await feature
            var lr = new LongRunning();

            // TcpServer feature (modificato anche metodo main)
            var test = new TestTcpServer();
            await test.Test2();
            
            // qui verrà aggiunto il riferimento al nuovo progetto BackgroundWorker
        }

        static int Add(int x1, int x2)
        {
            return x1 + x2;
        }
    }
}

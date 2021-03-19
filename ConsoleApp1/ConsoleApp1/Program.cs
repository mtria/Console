using System;
using AsyncAwaitConsoleTest;
using TcpServer;
using BackGroundWorkerTest;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        async static Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // questo commento è stato editato in GitHub
            // questo commento è stato aggiunto in VS

            // Async Await feature
            await ProvaAsyncAwait();

            // TcpServer feature (modificato anche metodo main)
            //var test = new TestTcpServer();
            //await test.Test2();

            // qui verrà aggiunto il riferimento al nuovo progetto BackgroundWorker
            //new Worker().Run();
        }

        async static Task ProvaAsyncAwait()
        {
            string s1 = null;
            var s2 = "stringa";
            if (s1 == s2)
            {
                Console.WriteLine("Le stringhe sono uguali");
            }
            var lr = new LongRunning();
            var stringa1 = await lr.GetString();
            Console.WriteLine(stringa1);

            Console.WriteLine("Stringa finale");

            Console.ReadKey();
        }

        static int Add(int x1, int x2)
        {
            return x1 + x2;
        }
    }
}

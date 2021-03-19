using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BackGroundWorkerTest
{
    public class Worker
    {
        public Server _server;

        public Worker()
        {
            _server = new Server();
            _server.ServerStart += (sender, e) => { Console.WriteLine("Server avviato"); };
            _server.DataReceived += (sender, e) =>
            {
                Console.WriteLine($"Il messaggio ricevuto è: {e.Request}");
                Console.WriteLine($"La risposta fornita è: {e.Response}");
            };
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("Worker in esecuzione...");
                Thread.Sleep(3000);

                Thread t = new Thread(() =>
                {
                    _server.StartListening();
                });
                t.Start();
            }
        }
    }
}

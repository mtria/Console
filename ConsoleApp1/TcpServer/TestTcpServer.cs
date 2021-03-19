using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpServer
{
    public class TestTcpServer
    {
        public async Task Test()
        {
            using (var server = new TcpServer(IPAddress.Any, 54001))
            {
                server.OnDataReceived += async (sender, e) =>
                {
                    var bytesRead = 0;
                    do
                    {
                        // Read buffer, discarding data
                        bytesRead = e.Stream.Read(new byte[1024], 0, 1024);
                    }
                    while (bytesRead > 0 && e.Stream.DataAvailable);

                    // Simulate long running task
                    Console.WriteLine($"Doing some heavy response processing now. [{Thread.CurrentThread.ManagedThreadId}]");
                    await Task.Delay(3000);
                    Console.WriteLine($"Finished processing. [{Thread.CurrentThread.ManagedThreadId}]");

                    var response = Encoding.ASCII.GetBytes("Who's there?");
                    e.Stream.Write(response, 0, response.Length);
                };

                await Task.Run(async () =>
                {
                    var serverTask = server.StartAsync();

                    var tasks = new List<Task>();

                    for (var i = 0; i < 5; ++i)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            var response = new byte[1024];

                            using (var client = new TcpClient())
                            {
                                client.Connect("127.0.0.1", 54001);

                                using (var stream = client.GetStream())
                                {
                                    var request = Encoding.ASCII.GetBytes("Knock, knock...");
                                    stream.Write(request, 0, request.Length);
                                    stream.Read(response, 0, response.Length);

                                    //Assert.AreEqual("Who's there?", Encoding.ASCII.GetString(response).TrimEnd('\0'));
                                    Console.WriteLine($"Who's there? Echo: " + Encoding.ASCII.GetString(response).TrimEnd('\0') + $" [{Thread.CurrentThread.ManagedThreadId}]");
                                }
                            }
                        }));
                    }

                    //Assert.IsTrue(Task.WaitAll(tasks.ToArray(), 10000));
                    //Console.WriteLine($"IsTrue: " + Task.WaitAll(tasks.ToArray(), 10000));
                    Parallel.ForEach(tasks, t =>
                    {
                        t.Wait();
                    });

                    await serverTask;
                });

            }
        }

        public async Task Test2()
        {
            var server = new TcpServer(IPAddress.Any, 54001);

            server.OnDataReceived += async (sender, e) =>
            {
                var bytesRead = 0;
                var buffer = new byte[1024];
                do
                {
                    // Read buffer, discarding data
                    bytesRead = e.Stream.Read(buffer, 0, 1024);
                }
                while (bytesRead > 0 && e.Stream.DataAvailable);

                var request = Encoding.ASCII.GetString(buffer).TrimEnd('\0');

                // Simulate long running task
                Console.WriteLine($"Messaggio in ingresso: {request} sul thread {Thread.CurrentThread.ManagedThreadId}");
                await Task.Delay(3000);
                Console.WriteLine($"Fine elaborazione messaggio {request} sul thread {Thread.CurrentThread.ManagedThreadId}");

                var response = Encoding.ASCII.GetBytes($"Risposta al messaggio '{request}'");
                e.Stream.Write(response, 0, response.Length);
            };

            var tasks = new List<Task>();

            for (int i = 0; i < 5; i++)
            {
                tasks.Add(SendMessage($"Toc Toc sono il numero {(i + 1)}"));
            }

            await server.StartAsync();

            Parallel.ForEach(tasks, t =>
            {
                t.Wait();
            });
        }

        private Task SendMessage(string message)
        {
            return Task.Run(() =>
            {
                var response = new byte[1024];

                using (var client = new TcpClient())
                {
                    client.Connect("127.0.0.1", 54001);

                    using (var stream = client.GetStream())
                    {
                        var request = Encoding.ASCII.GetBytes(message);
                        stream.Write(request, 0, request.Length);
                        stream.Read(response, 0, response.Length);

                        var sResponse = $"Ecco la risposta al messaggio {message}: {Encoding.ASCII.GetString(response).TrimEnd('\0')}";
                        Console.WriteLine(sResponse);
                    }
                }
            });
        }
    }
}

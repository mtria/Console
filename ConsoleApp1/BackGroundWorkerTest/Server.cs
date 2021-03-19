using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BackGroundWorkerTest
{
    public class Server
    {
        private readonly TcpListener _server;
        public event EventHandler ServerStart;
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public Server()
        {
            _server = new TcpListener(IPAddress.Parse("127.0.0.1"), 54001);
            _server.Start();
        }

        public void StartListening()
        {
            try
            {
                ServerStart?.Invoke(this, new EventArgs());
                TcpClient client = _server.AcceptTcpClient();

                Thread t = new Thread(new ParameterizedThreadStart(HandleMessage));
                t.Start(client);

            }
            catch (Exception ex)
            {
                _server.Stop();
            }
        }

        private void OnServerStart()
        {
            if (ServerStart != null)
            {
                var delegates = ServerStart.GetInvocationList();
                for (int i = 0; i < delegates.Length; i++)
                {
                    ((EventHandler)delegates[i]).BeginInvoke(this, new EventArgs(), null, null);
                }
            }
        }

        public void HandleMessage(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();

            string sRequest;
            string sResponse;
            var buffer = new byte[1024];
            int bytesRead;
            try
            {
                do
                {
                    bytesRead = stream.Read(buffer, 0, 1024);
                }
                while (bytesRead > 0 && stream.DataAvailable);

                sRequest = Encoding.ASCII.GetString(buffer).TrimEnd('\0');
                sResponse = $"Ho ricevuto il messaggio ({sRequest})";

                var bResponse = Encoding.ASCII.GetBytes(sResponse);

                // invio la risposta al client
                stream.Write(bResponse, 0, bResponse.Length);

                // notifico
                DataReceived?.Invoke(this, new DataReceivedEventArgs(sRequest, sResponse));
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class DataReceivedEventArgs : EventArgs
    {
        public string Request { get; set; }
        public string Response { get; set; }

        public DataReceivedEventArgs(string request, string response)
        {
            this.Request = request;
            this.Response = response;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using app.ServerLogic;
using app.Storage;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            Int32 port = 30000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            var tcpServer = new TcpListener(localAddr, port);

            var cache = new LRUStringCache(capacity: 5);
            var server = new Server(cache);
            tcpServer.Start();

            while (true)
            {
                var client = tcpServer.AcceptTcpClient();
                server.HandleNewStream(client.GetStream());
            }
        }
    }
}

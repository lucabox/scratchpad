using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using app.Storage;

namespace app.ServerLogic
{
    public class Server
    {
        public IStringCache _cache;

        public Server(IStringCache cache)
        {
            _cache = cache;
        }

        public enum Commands
        {
            PUT,
            GET
        }

        public enum Status
        {
            Ok,
            Fail
        }

        /// <summary>
        ///  handle a new inbound stream, using a Stream for testing purposes (can use an InMemoryStream)
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task HandleNewStream(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.ASCII))
            {
                string line;
                do
                {
                    line = await reader.ReadLineAsync();
                    if (line != null)
                    {
                        Commands command;
                        byte[] response = Response(Status.Fail);
                        if (Enum.TryParse(line, false, out command))
                        {
                            var key = await reader.ReadLineAsync();
                            if (!string.IsNullOrEmpty(key))
                            {
                                switch (command)
                                {
                                    case Commands.GET:
                                        Console.WriteLine("got a get");
                                        var get = new Get(key: key);
                                        var valueGot = await Handle(get);
                                        response = Response(valueGot);
                                        break;
                                    case Commands.PUT:
                                        Console.WriteLine("got a put");
                                        var value = await reader.ReadLineAsync();
                                        if (!string.IsNullOrEmpty(key))
                                        {
                                            var put = new Put(key: key, value: value);
                                            await Handle(put);
                                            response = Response(Status.Ok);
                                        }
                                        break;
                                    default:
                                        Console.WriteLine("got something else...");
                                        response = Response(Status.Fail);
                                        break;
                                }
                            }
                        }
                        stream.Write(response, 0, response.Length);
                    }
                } while (line != null);
            }
        }

        public async Task Handle(Put put)
        {
            Console.WriteLine("Handling a put");

            // a simple lock for simplicity
            // this should be a reader writer lock instead
            // or we can partition the key space to have a lock per partition
            lock (_cache)
            {
                _cache.Put(put.Key, put.Value);
            }
            await Task.FromResult(0);
        }

        public async Task<string> Handle(Get get)
        {
            // a simple lock for simplicity
            // this should be a reader writer lock instead
            // or we can partition the key space to have a lock per partition
            string value;
            lock (_cache)
            {
                value = _cache.Get(get.Key);
            }
            return value;
        }

        private static byte[] Response(Status status)
        {
            return Response(status.ToString());
        }

        private static byte[] Response(string value)
        {
            return Encoding.ASCII.GetBytes(value + "\n");
        }

        static public Stream AsStream(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}

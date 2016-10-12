using System;
using System.IO;
using app.ServerLogic;
using app.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace appTest
{
    [TestFixture]
    public class UnitTest1
    {
        static public Stream AsStream(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        [Test]
        public void TestHandleStream()
        {

            var input = "PUT\nhello\nsomevalue\n";

            var server = new Server(new LRUStringCache(5));

            server.HandleNewStream(AsStream(input));

            // incomplete!
        }
    }
}

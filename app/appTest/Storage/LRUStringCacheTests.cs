using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.Storage;
using NUnit.Framework;

namespace appTest.Storage
{
    
    [TestFixture]
    public class LRUStringCacheTests
    {
        public LRUStringCache Cache { get; set; }

        public int Capacity { get; set; }

        [SetUp]
        public void SetUp()
        {
            Capacity = 2;
            Cache = new LRUStringCache(Capacity);
        }

        [Test]
        public void TestPutAndGet()
        {
            Cache.Put(key: "hello", value: "someValue");
            var value = Cache.Get("hello");
            Assert.That(value == "someValue");
        }

        [Test]
        public void TestGetNonExistingValue()
        {
            Cache.Put(key: "hello", value: "someValue");
            
            var value = Cache.Get("notthekeyyourelookingfor");

            Assert.That(value == string.Empty);
        }

        [Test]
        public void TestLimit()
        {
            Capacity = 2;
            Cache = new LRUStringCache(Capacity);

            Cache.Put(key: "hello", value: "someValue");
            Cache.Put(key: "hello1", value: "someValue1");
            Cache.Put(key: "hello2", value: "someValue2");
            Cache.Put(key: "hello3", value: "someValue3");
            Cache.Put(key: "hello4", value: "someValue4");

            Assert.That(Cache.Get("hello") == string.Empty); // evicted
            Assert.That(Cache.Get("hello1") == string.Empty); // evicted
            Assert.That(Cache.Get("hello2") == string.Empty); // evicted
            Assert.That(Cache.Get("hello3") == "someValue3"); // still there!
            Assert.That(Cache.Get("hello4") == "someValue4"); // still there!
        }
    }
}

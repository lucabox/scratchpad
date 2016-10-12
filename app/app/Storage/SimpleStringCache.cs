using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Storage
{
    /// <summary>
    /// A non thread safe implementation of a limited size cache.
    /// 
    /// </summary>
    public class SimpleStringCache : IStringCache
    {
        private int _capacity;

        private Dictionary<string, string> _keyValues;

        private Queue<string> _keys;

        public SimpleStringCache(int capacity)
        {
            if (capacity <= 1)
            {
                throw new ArgumentException("capacity has to be at least 1");
            }

            _capacity = capacity;
            _keyValues = new Dictionary<string, string>(_capacity);
            _keys = new Queue<string>(_capacity);
        }

        /// <summary>
        /// Add an item to the cache
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="value"></param>
        public void Put(string key, string value)
        {
            if (_keyValues.ContainsKey(key))
            {
                // we already know this key
                // just replace its value
                // its age stays the same
                _keyValues[key] = value;
            }
            else
            {
                // it's a new key
                if (_keys.Count == _capacity)
                {
                    // and we are at capacity, evict oldest key
                    var evictedKey = _keys.Dequeue();
                    _keyValues.Remove(evictedKey);
                }

                _keys.Enqueue(key);
                _keyValues[key] = value;
            }
        }

        /// <summary>
        /// Get a key from cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            if (_keyValues.ContainsKey(key))
            {
                return _keyValues[key];
            }

            return string.Empty;
        }
    }
}

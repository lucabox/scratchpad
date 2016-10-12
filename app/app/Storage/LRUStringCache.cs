using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Storage
{
    public class LRUStringCache : IStringCache
    {
        private int _capacity;

        private Dictionary<string, LinkedListNode<Tuple<string, string>>> _keyNodes;

        private LinkedList<Tuple<string, string>> _keys;

        public LRUStringCache(int capacity)
        {
            if (capacity <= 1)
            {
                throw new ArgumentException("capacity has to be at least 1");
            }

            _capacity = capacity;
            _keyNodes = new Dictionary<string, LinkedListNode<Tuple<string, string>>>(_capacity);
            _keys = new LinkedList<Tuple<string, string>>();
        }

        public void Put(string key, string value)
        {
            var node = DoGet(key);
            if (node != null)
            {
                node.Value = new Tuple<string, string>(key, value);
                return;
            }

            // we need to add a new node
            var newNode = new LinkedListNode<Tuple<string, string>>(new Tuple<string, string>(key, value));

            if (_keys.Count == _capacity)
            {
                // we evict oldest node
                var oldestNode = _keys.Last;
                _keys.Remove(oldestNode);
                _keyNodes.Remove(oldestNode.Value.Item1);

            }

            _keyNodes.Add(key: key, value: newNode);
            _keys.AddFirst(newNode);
        }

        public string Get(string key)
        {
            var node = DoGet(key);
            return (node != null) ? node.Value.Item2 : string.Empty;
        }

        private LinkedListNode<Tuple<string, string>> DoGet(string key)
        {
            if (_keyNodes.ContainsKey(key))
            {
                // we know the key
                var node = _keyNodes[key];
                _keys.Remove(node);
                _keys.AddFirst(node);
                return node;
            }

            return null;
        }
    }
}

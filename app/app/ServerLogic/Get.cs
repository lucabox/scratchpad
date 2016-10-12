using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.ServerLogic
{
    public class Get
    {
        public Get(string key)
        {
            Key = key;
        }

        public string Key { get; private set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Storage
{
    public interface IStringCache
    {
        void Put(string key, string value);

        string Get(string key);
    }
}

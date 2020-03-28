﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParasiteIO.Core.Types
{

   
    public interface IParasiteObject
    {
        string TypeName { get; }
        Dictionary<string,string> Properties { get; set; }
    }
}

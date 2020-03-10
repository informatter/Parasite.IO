using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parasite.Core.Types;

namespace Parasite.Core.Data
{
    public sealed class DataContainerFactory
    {
        public  List<object>  data;
        public DataContainerFactory(List<object> _data)
        {
            data = _data;

        }
    }
}

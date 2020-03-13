using Parasite.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Core.Collections
{

    [Serializable]
    public struct DataNode<T> where T: ParasiteObject
    {
        ParasiteObject m_Data;
        public DataNode( ParasiteObject data)
        {
            m_Data = data;
        }

        public ParasiteObject Node { get => m_Data; }
    }
}

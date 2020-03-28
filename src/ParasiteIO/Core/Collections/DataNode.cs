
using System;


using ParasiteIO.Core.Types;

namespace ParasiteIO.Core.Collections
{

    [Serializable]
    public struct DataNode<T> where T: ParasiteAbstractObject
    {
        ParasiteAbstractObject m_Data;
        public DataNode( ParasiteAbstractObject data)
        {
            m_Data = data;
        }

        public ParasiteAbstractObject Node { get => m_Data; }
    }
}

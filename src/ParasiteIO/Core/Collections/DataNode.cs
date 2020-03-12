using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parasite.Core.Types;


namespace Parasite.Core.Collections
{

    [Serializable]  
    public struct DataNode<T>: IEquatable<DataNode<T>>
        where T: ParasiteObject 
    {
      
        /// <summary>
        /// 
        /// </summary>
        private T m_data;

        public DataNode(T data) => m_data = data;

        public T Node { get => m_data; }

        public bool Equals(DataNode<T> other)
        {
            if (!m_data.Equals(other.Node)) { return false; }
            return true;
        }
    }
}

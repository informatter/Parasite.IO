using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parasite.Core.Types;


namespace Parasite.Core.Collections
{

    [Serializable]
    public class DataContainer
    {
       

        private Dictionary<int,DataNode<ParasiteObject>> m_Data;

        public Dictionary<int, DataNode<ParasiteObject>> Data { get => m_Data; }

        public DataContainer() => m_Data = new Dictionary<int, DataNode<ParasiteObject>>();
   

      
    }
}

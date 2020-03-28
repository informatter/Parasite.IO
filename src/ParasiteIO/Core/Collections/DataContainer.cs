using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ParasiteIO.Core.Types;


namespace ParasiteIO.Core.Collections
{
    /// <summary>
    /// 
    /// </summary>

    [Serializable]
    public struct DataContainer
    {


        private DataNode<ParasiteAbstractObject>[][] m_Data;

        //private string m_id;

        public DataContainer(int n) => m_Data = new DataNode<ParasiteAbstractObject>[n][];

        public DataNode<ParasiteAbstractObject>[][] Data { get => m_Data; }

        // public string ID { get => m_id; }



    }
}

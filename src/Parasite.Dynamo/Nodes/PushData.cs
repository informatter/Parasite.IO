using Autodesk.DesignScript.Geometry;
using Parasite.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Dynamo.Nodes
{

    /// <summary>
    /// This class has the necessary method to Push data From Dynamo 
    /// To a Server or locally on disk
    /// </summary>
    public class PushData
    {
        private PushData() { }



        public static void Push(/*List<DesignScriptEntity> data*/ params List<DesignScriptEntity>[] data)
        {
            //List<DataContainerFactory> _dataContainers = new List<DataContainerFactory>();

            //for (int i = 0; i < data.Count; i++)
            //{

            //}
        }

    }
}

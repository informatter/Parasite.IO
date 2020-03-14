using System;
//using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Parasite.Core.Collections;
using Parasite.Core.Data.CollectData;
using Parasite.Core.Sync;

namespace Parasite.Dynamo
{

    /// <summary>
    /// This class has the necessary method to Push data From Dynamo 
    /// To a Server or locally on disk
    /// </summary>
    public class ParasiteSender
    {
        private readonly CollectDataFromDyn fromDynamo = new CollectDataFromDyn();
        /// <summary>
        /// 
        /// </summary>
        public ParasiteSender(string id, params List<object>[] data)
        {
            Push(id, data);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="data"></param>
        /// <returns>Pushed @ </returns>
        [IsVisibleInDynamoLibrary(false)]
        private string Push(string ID, params List<object>[] data)
        {
            List<List<object>> dataContainers = new List<List<object>>();


            for (int i = 0; i < data.Count(); i++)
            {
                List<object> d = new List<object>();
                for (int j = 0; j < data[i].Count; j++)
                {

                    if (data[i][j] == null) continue;

                    d.Add(data[i][j]);

                }

                dataContainers.Add(d);
            }

            DataContainer dc = fromDynamo.CollectData(dataContainers);

            PushData pd = new PushData();

            pd.PushDataLocal(dc, ID);

            return "Pushed @" + Environment.NewLine + string.Format("{0:HH:mm:ss tt}", DateTime.Now);
        }

    }
}
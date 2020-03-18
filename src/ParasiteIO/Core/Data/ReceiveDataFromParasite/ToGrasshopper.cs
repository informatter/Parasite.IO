using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parasite.Core.Collections;
using Parasite.Core.Sync;
using Parasite.Core.Types.Geometry;
using Parasite.Core.Exceptions;
using Parasite.Conversion.Rhinoceros;

namespace Parasite.Core.Data.ReceiveDataFromParasite
{

    /// <summary>
    /// This class Receives data From Parasite and 
    /// converts it to Rhino/Grasshopper Geometry
    /// </summary>
    public class ToGrasshopper // : IReceiveData
    {

        /// <summary>
        /// Receives data and performs the appropriate type conversion
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public /*List<object>*/  List<List<object>> ReceiveData(string ID, double RhinoDocTol)
        {
            //List<object> outPut = new List<object>();

            List<List<object>> outPut = new List<List<object>>();


            RequestData rd = new RequestData();
            DataContainer dataContainer = rd.RequestDataLocal(ID);

            for (int i = 0; i < dataContainer.Data.Length; i++)
            {
                List<object> data = new List<object>();

                for (int j = 0; j < dataContainer.Data[i].Length; j++)
                {
                    if (dataContainer.Data[i][j].Node == null) continue;

                    if (dataContainer.Data[i][j].Node is Parasite_Mesh mesh)
                    {
                        throw new ParasiteNotImplementedExceptions(" type Not implemented Yet!");
                    }

                    else if (dataContainer.Data[i][j].Node is Parasite_Sphere sph)
                    {
                       // outPut.Add(RhinoConversion.ToRhinoType(sph));
                    }

                    else if (dataContainer.Data[i][j].Node is Parasite_BrepSolid brepSolid)
                    {
                       // outPut.Add(RhinoConversion.ToRhinoType(brepSolid, RhinoDocTol));

                        data.Add(RhinoConversion.ToRhinoType(brepSolid, RhinoDocTol));
                    }

                    else
                    {
                        throw new ParasiteNotImplementedExceptions(" type Not implemented Yet!");
                    }
                }

                outPut.Add(data);
            }




            return outPut;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino.Geometry;

using ParasiteIO.Core.Collections;
using ParasiteIO.Core.Sync;
using ParasiteIO.Core.Types.Geometry;
using ParasiteIO.Core.Exceptions;
using ParasiteIO.Conversion.Rhinoceros;
using ParasiteIO.Core.Types;

namespace ParasiteIO.Core.Data.ReceiveDataFromParasite
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
        public  List<List<object>> ReceiveData(string ID, double RhinoDocTol)
        {
                       
            List<List<object>> outPut = new List<List<object>>();
            RequestData rd = new RequestData();
            DataContainer dataContainer = rd.RequestDataLocal(ID);

            for (int i = 0; i < dataContainer.Data.Length; i++)
            {
                List<object> data = new List<object>();

                for (int j = 0; j < dataContainer.Data[i].Length; j++)
                {
                    if (dataContainer.Data[i][j].Node == null) continue;

                   else  if(dataContainer.Data[i][j].Node is ParasiteObject pObj)                    
                        data.Add(pObj);
                    

                   else if (dataContainer.Data[i][j].Node is Parasite_Mesh mesh)                  
                        data.Add(RhinoConversion.ToRhinoType(mesh));
                    

                    else if (dataContainer.Data[i][j].Node is Parasite_Sphere sph)
                    {
                        throw new ParasiteNotImplementedExceptions(" type Not implemented Yet!");
                    }

                    else if (dataContainer.Data[i][j].Node is Parasite_BrepSolid brepSolid)
                        data.Add(RhinoConversion.ToRhinoType(brepSolid, RhinoDocTol));
                    

                    else                   
                        throw new ParasiteNotImplementedExceptions(" type Not implemented Yet!");
                    
                }

                outPut.Add(data);
            }




            return outPut;
        }
    }
}

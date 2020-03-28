using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Geometry;
using ParasiteIO.Conversion.Parasite;
using ParasiteIO.Core.Types;
using ParasiteIO.Core.Types.Geometry;
using ParasiteIO.Core.Exceptions;
using ParasiteIO.Core.Collections;
using ParasiteIO.Core.Data.CollectDataFromApplication;


namespace ParasiteIO.Core.Data.CollectData
{
    public class CollectDataFromDyn : IApplicationDataCollector
    {
        public DataContainer CollectData(List<List<object>>  dataFromApp)
        {
            DataContainer dataContainer = new DataContainer( dataFromApp.Count);

            for (int i = 0; i < dataFromApp.Count; i++)
            {
                DataNode<ParasiteAbstractObject>[] nodeArray = new DataNode<ParasiteAbstractObject>[ dataFromApp[i].Count];

                for (int j = 0; j < dataFromApp[i].Count; j++)
                {
                    if (dataFromApp[i][j] == null) continue;

                    else if (dataFromApp[i][j] is ParasiteObject pObj)
                    {
                        if (pObj.Data is Solid)
                        {
                            Solid s = pObj.Data as Solid;

                            Parasite_BrepSolid pSolid = ParasiteConversion.ToParasiteType(s);

                            pObj.Data = pSolid;
                            nodeArray[j] = new DataNode<ParasiteAbstractObject>(pObj);
                        }
                    }

                    else if (dataFromApp[i][j] is Point p)
                    {
                        Parasite_Point3d point = ParasiteConversion.ToParasiteType(p);
                        nodeArray[j] = new DataNode<ParasiteAbstractObject>(point);

                    }

                    else if (dataFromApp[i][j] is Sphere sph)
                    {
                        Parasite_Sphere s = new Parasite_Sphere(ParasiteConversion.ToParasiteType(sph.CenterPoint), sph.Radius);
                        nodeArray[j] = new DataNode<ParasiteAbstractObject>(s);

                    }

                    else if (dataFromApp[i][j] is Solid solid)
                    {
                        Parasite_BrepSolid pSolid = ParasiteConversion.ToParasiteType(solid);
                        nodeArray[j] = new DataNode<ParasiteAbstractObject>(pSolid);
                    }

                    //else
                    //    throw new ParasiteNotImplementedExceptions("Object type not implemented yet!");


                }

                dataContainer.Data[i] = nodeArray;
            }

            return dataContainer;
        }
    }
}

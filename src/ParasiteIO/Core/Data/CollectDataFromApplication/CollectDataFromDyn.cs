using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Geometry;
using Parasite.Conversion.Parasite;
using Parasite.Core.Types;
using Parasite.Core.Types.Geometry;
using Parasite.Core.Exceptions;
using Parasite.Core.Collections;
using Parasite.Core.Data.CollectDataFromApplication;

namespace Parasite.Core.Data.CollectData
{
    public class CollectDataFromDyn : IApplicationDataCollector
    {
        public DataContainer CollectData(List<List<object>>  dataFromApp)
        {
            DataContainer dataContainer = new DataContainer( dataFromApp.Count);

            for (int i = 0; i < dataFromApp.Count; i++)
            {
                DataNode<ParasiteObject>[] nodeArray = new DataNode<ParasiteObject>[ dataFromApp[i].Count];

                for (int j = 0; j < dataFromApp[i].Count; j++)
                {
                    if (dataFromApp[i][j] == null) continue;


                    if (dataFromApp[i][j] is Point p)
                    {
                        Parasite_Point3d point = ParasiteConversion.ToParasiteType(p);
                        nodeArray[j] = new DataNode<ParasiteObject>(point);

                    }

                    else if (dataFromApp[i][j] is Sphere sph)
                    {
                        Parasite_Sphere s = new Parasite_Sphere(ParasiteConversion.ToParasiteType(sph.CenterPoint), sph.Radius);
                        nodeArray[j] = new DataNode<ParasiteObject>(s);

                    }

                    else if (dataFromApp[i][j] is Solid solid)
                    {
                        Parasite_BrepSolid pSolid = ParasiteConversion.ToParasiteType(solid);
                        nodeArray[j] = new DataNode<ParasiteObject>(pSolid);
                    }

                    else
                        throw new ParasiteNotImplementedExceptions("Object type not implemented yet!");


                }

                dataContainer.Data[i] = nodeArray;
            }

            return dataContainer;
        }
    }
}

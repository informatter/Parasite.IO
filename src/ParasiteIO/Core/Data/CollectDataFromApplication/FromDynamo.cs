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
    public class FromDynamo : IApplicationDataCollector
    {
        public DataContainer CollectDataFromApplication(List<DataContainerFactory> dataFromApp)
        {
            DataContainer dataContainer = new DataContainer();

            for (int i = 0; i < dataFromApp.Count; i++)
            {

                for (int j = 0; j < dataFromApp[i].data.Count; j++)
                {

                    if (dataFromApp[i].data[j] is Point p)
                    {
                        Parasite_Point3d point = ParasiteConversion.ToParasiteType(p);
                        DataNode<ParasiteObject> node = new DataNode<ParasiteObject>(point);
                        dataContainer.Data.Add(j, node);
                    }

                    else if (dataFromApp[i].data[j] is Sphere sph)
                    {
                        Parasite_Sphere s = new Parasite_Sphere(ParasiteConversion.ToParasiteType(sph.CenterPoint), sph.Radius);
                        DataNode<ParasiteObject> node = new DataNode<ParasiteObject>(s);
                        dataContainer.Data.Add(j, node);
                    }

                    else
                        throw new ParasiteNotImplementedExceptions("Object type not implemented yet!");


                }
            }

            return dataContainer;
        }
    }
}

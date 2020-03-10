using System;
using System.Collections.Generic;
using Grasshopper.Kernel.Types;
using Parasite.Core.Exceptions;
using Parasite.Core.Types.Geometry;
using Rhino.Geometry;
using Rhino.Geometry.Collections;

using Parasite.Conversion.Parasite;
using Parasite.Core.Types;
using System.Drawing;

namespace Parasite.Core.Data.CollectData
{

    /// <summary>
    /// 
    /// </summary>
    public  class FromGrasshopper
    {

        public  static DataContainer RetreiveData(List<DataContainerFactory> dataFromApp)
        {
            DataContainer dataContainer = new DataContainer();

            for (int i = 0; i < dataFromApp.Count; i++)
            {

                for (int j = 0; j < dataFromApp[i].data.Count; j++)
                {

                    if (dataFromApp[i].data[j] is GH_Point p)
                    {
                        if (p.CastTo(out Point3d pt))
                        {
                            Parasite_Point3d point = ParasiteConversion.ToParasiteType(pt);

                            DataNode<ParasiteObject> node = new DataNode<ParasiteObject>(point);

                            dataContainer.Data.Add(j, node);
                        }
                    }

                    else if (dataFromApp[i].data[j] is GH_Surface srf)
                    {
                        Brep brep = srf.Value;

                        if (brep.IsSurface)
                        {
                            BrepSurfaceList srfList = brep.Surfaces;

                            if (srfList.Count == 1)
                            {
                              
                                Parasite_NurbsSurface point = ParasiteConversion.ToParasiteType(srfList[0].ToNurbsSurface());

                                DataNode<ParasiteObject> node = new DataNode<ParasiteObject>(point);

                                dataContainer.Data.Add(j, node);
                            }
                        }
                    }


                    else if (dataFromApp[i].data[j] is GH_Brep b)
                    {
                        Brep brep = b.Value;

                        if (brep.IsSurface && !brep.IsSolid)
                        {
                            
                            Parasite_BrepSurface parasiteBrepSrf = ParasiteConversion.ToParasiteType(brep);

                            DataNode<ParasiteObject> node = new DataNode<ParasiteObject>(parasiteBrepSrf);

                            dataContainer.Data.Add(j, node);
                        }

                        if (!brep.IsSurface && brep.IsSolid)
                        {
                           // dataContainer.Data.Add(new Parasite_BrepSolid(brep));
                        }
                    }

                    else if (dataFromApp[i].data[j] is GH_Mesh m)
                    {
                        Rhino.Geometry.Mesh mesh = m.Value;

                        Parasite_Mesh parasiteMesh = ParasiteConversion.ToParasiteType(mesh);

                        DataNode<ParasiteObject> node = new DataNode<ParasiteObject>(parasiteMesh);

                        dataContainer.Data.Add(j, node);

            
                    }

                    
                    /// CONNVERT GH_CURVE TO PARASITE CURVE
                    else if (dataFromApp[i].data[j] is GH_Curve curve)
                    {
                        Rhino.Geometry.NurbsCurve nc = curve.Value as NurbsCurve;

                        if (nc.IsArc())
                        {
                            throw new NotImplementedException();
                        }

                        else if (nc.IsCircle())
                        {
                            throw new NotImplementedException();
                        }

                        else if (nc.IsEllipse())
                        {
                            throw new NotImplementedException();

                        }

                        else if (nc.IsPolyline())
                        {
                            throw new NotImplementedException();
                        }

                        else
                        {

                            Parasite_NurbsCurve parasiteNurbsCurve = ParasiteConversion.ToParasiteType(nc);

                            DataNode<ParasiteObject> node = new DataNode<ParasiteObject>(parasiteNurbsCurve);

                            dataContainer.Data.Add(j, node);
                        }



                    }

                  

                    else
                    {
                        throw new ParasiteNotImplementedExceptions("Type conversion not implemented yet!");
                    }
                }



            }

            return dataContainer;
        }
    }
}

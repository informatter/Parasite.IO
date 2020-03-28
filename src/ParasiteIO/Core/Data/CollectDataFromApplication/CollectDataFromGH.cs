using System;
using System.Drawing;
using System.Collections.Generic;

using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Rhino.Geometry.Collections;


using ParasiteIO.Core.Collections;
using ParasiteIO.Core.Exceptions;
using ParasiteIO.Core.Types.Geometry;
using ParasiteIO.Conversion.Parasite;
using ParasiteIO.Core.Types;

namespace ParasiteIO.Core.Data.CollectDataFromApplication
{

    /// <summary>
    /// 
    /// </summary>
    public class CollectDataFromGH : IApplicationDataCollector
    {

        public DataContainer CollectData(List<List<object>> dataFromApp)
        {
          
            DataContainer dataContainer = new DataContainer(dataFromApp.Count);

            for (int i = 0; i < dataFromApp.Count; i++)
            {
                DataNode<ParasiteAbstractObject>[] nodeArray = new DataNode<ParasiteAbstractObject>[dataFromApp[i].Count];

                for (int j = 0; j < dataFromApp[i].Count; j++)
                {

                    if (dataFromApp[i][j] is GH_Point p)
                    {
                        if (p.CastTo(out Point3d pt))
                        {
                            Parasite_Point3d point = ParasiteConversion.ToParasiteType(pt);
                            nodeArray[j] = new DataNode<ParasiteAbstractObject>(point);
                        }
                    }

                    else if (dataFromApp[i][j] is GH_Surface srf)
                    {
                        Brep brep = srf.Value;

                        if (brep.IsSurface)
                        {
                            BrepSurfaceList srfList = brep.Surfaces;

                            if (srfList.Count == 1)
                            {

                                Parasite_NurbsSurface paraSrf = ParasiteConversion.ToParasiteType(srfList[0].ToNurbsSurface());
                                nodeArray[j] = new DataNode<ParasiteAbstractObject>(paraSrf);
                            }
                        }
                    }


                    else if (dataFromApp[i][j] is GH_Brep b)
                    {
                        Brep brep = b.Value;

                        if (brep.IsSurface && !brep.IsSolid)
                        {

                            Parasite_BrepSurface parasiteBrepSrf = ParasiteConversion.ToParasiteType(brep);
                            nodeArray[j] = new DataNode<ParasiteAbstractObject>(parasiteBrepSrf);
                        }

                        if (!brep.IsSurface && brep.IsSolid)
                        {
                            // dataContainer.Data.Add(new Parasite_BrepSolid(brep));
                        }
                    }

                    else if (dataFromApp[i][j] is GH_Mesh m)
                    {
                        Rhino.Geometry.Mesh mesh = m.Value;

                        Parasite_Mesh parasiteMesh = ParasiteConversion.ToParasiteType(mesh);
                        nodeArray[j] = new DataNode<ParasiteAbstractObject>(parasiteMesh);
                    }


                    /// CONNVERT GH_CURVE TO PARASITE CURVE
                    else if (dataFromApp[i][j] is GH_Curve curve)
                    {
                        Rhino.Geometry.NurbsCurve nc = curve.Value as NurbsCurve;

                        if (nc.IsArc())
                        {
                            throw new ParasiteNotImplementedExceptions("Object type not implemented yet!");
                        }

                        else if (nc.IsCircle())
                        {
                            throw new ParasiteNotImplementedExceptions("Object type not implemented yet!");
                        }

                        else if (nc.IsEllipse())
                        {
                            throw new ParasiteNotImplementedExceptions("Object type not implemented yet!");

                        }

                        else if (nc.IsPolyline())
                        {
                            throw new ParasiteNotImplementedExceptions("Object type not implemented yet!");
                        }

                        else
                        {

                            Parasite_NurbsCurve parasiteNurbsCurve = ParasiteConversion.ToParasiteType(nc);
                            nodeArray[j] = new DataNode<ParasiteAbstractObject>(parasiteNurbsCurve);
                        }



                    }



                    else
                    {
                        throw new ParasiteNotImplementedExceptions("Type conversion not implemented yet!");
                    }
                }


                dataContainer.Data[i] = nodeArray;
            }

            return dataContainer;
        }
    }
}
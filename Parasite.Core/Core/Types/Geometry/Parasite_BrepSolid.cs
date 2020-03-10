
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parasite.Conversion.Parasite;
using Parasite.Core.Exceptions;
using Rhino.Geometry;
using Rhino.Geometry.Collections;

namespace Parasite.Core.Types.Geometry
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Parasite_BrepSolid : ParasiteObject
    {
        public enum ParasiteBrepTypes { Cylinder, BrepSolid };

        #region FIELDS 

        ParasiteBrepTypes m_parasiteBrepTypes;
        Parasite_Point3d[][] m_vertices;
        #endregion

        #region CONSTRUCTORS

        public Parasite_BrepSolid(object brep, Dictionary<string, string> properties = null)
        {
            base.Properties = properties;
            base.TypeName = GetType().Name;
            Foo(brep);
        }
        #endregion

        #region PROPERTIES
        public Parasite_Point3d [][] Vertices { get => m_vertices; }

        #endregion

        #region METHODS

        protected  void Foo(object geo)
        {
            if (geo is Rhino.Geometry.Brep)
            {
                Rhino.Geometry.Brep brep = geo as Rhino.Geometry.Brep;

                if (!brep.IsValid)
                {
                    Type RhinoBRep = brep.GetType();
                    Type ParasiteBRep = this.GetType();
                    throw new ParasiteConversionExceptions(RhinoBRep, ParasiteBRep,
                    string.Format("Could not convert from {0} to {1} please input a valid Brep ", RhinoBRep, ParasiteBRep));
                }

                if(!brep.IsSolid)
                    throw new ParasiteArgumentException(string.Format("{0} only accepts a solid Brep", brep.GetType().Name));

                else
                {
                    BrepSurfaceList surfaces = brep.Surfaces;
                    m_vertices = new Parasite_Point3d[surfaces.Count][];

                    for (int i = 0; i < m_vertices.Length; i++)
                    {

                        Brep brepSurface = surfaces[i].ToBrep();

                        // Check if brep is cylinder

                        Cylinder cylinder = Cylinder.Unset;
                        if (surfaces[i].TryGetCylinder(out cylinder) && brepSurface != null)
                        {
                            m_parasiteBrepTypes = ParasiteBrepTypes.Cylinder;
                            //cylinder.

                            throw new NotImplementedException();
                        }


                        // if Not a cylinder, yet still a valid brep surface, proceed
                        if (brepSurface != null && cylinder.IsValid == false)
                        {
                            //m_vertices[i] = brepSurface.Vertices.Select(x => ToParasiteType(x.Location)).ToArray();
                        }


                    }


                }


            }

        }
        #endregion



    }
}

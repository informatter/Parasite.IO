using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parasite.Core.Types.Geometry;
using Rhino.Geometry;

using Parasite.Core.Exceptions;
using Rhino.Collections;

namespace Parasite.Conversion.Rhinoceros
{
    public class RhinoConversion
    {

        #region POINTS

        public static Point3d ToRhinoType(Parasite_Point3d p) => new Point3d(p.X, p.Y, p.Z);

        #endregion

        #region MESH


        #endregion


        #region CURVES,LINES,POLYLINES/POLYCURVES



        #region POLYCURVE



        #endregion


        #endregion

        #region SURFACES



        #endregion

        #region SOLID BREPS

        public static Sphere ToRhinoType(Parasite_Sphere sph) => new Sphere(ToRhinoType(sph.Center), sph.Radius);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="solid"></param>
        /// <returns></returns>
        public static Brep  ToRhinoType(Parasite_BrepSolid solid)
        {
            int n = solid.Faces.Length;
            double tol = 0.0001;
            Brep[] faces = new Brep[n];

            for (int i = 0; i < n; i++)
            {
                Parasite_Point3d [] verts = solid.Faces[i].Vertices.ToArray();

                if (verts.Length < 3) throw new ParasiteArgumentException("A Brep Face cant be constructed from less than 2 vertices!");

                else if(verts.Length == 3)
                {
                    Point3d a = ToRhinoType(verts[0]);
                    Point3d b = ToRhinoType(verts[1]);
                    Point3d c = ToRhinoType(verts[2]);
                    faces[i] =  Brep.CreateFromCornerPoints(a,b,c,tol);
                }

                else if (verts.Length == 4)
                {
                    Point3d a = ToRhinoType(verts[0]);
                    Point3d b = ToRhinoType(verts[1]);
                    Point3d c = ToRhinoType(verts[2]);
                    Point3d d = ToRhinoType(verts[3]);
                    faces[i] = Brep.CreateFromCornerPoints(a,b,c,d,tol);                  
                }

                /// Build curve from vertices to then build a Brep face
                else
                {
                    //CurveList curveList = new CurveList(/*verts.Length*/);

                  Curve [] curveList = new Curve [verts.Length];
                    for (int j = 0; j < curveList.Length; j++)
                    {
                        if (j < curveList.Length - 1)
                        {
                           
                            Line ln = new Line(ToRhinoType(verts[j]),
                                ToRhinoType(verts[j + 1]));
                            curveList[j] = ln.ToNurbsCurve() as Curve;                  
                        }

                        if (j == curveList.Length-1)
                        {
                            Line ln = new Line(ToRhinoType(verts[j]),
                               ToRhinoType(verts[0]));

                            curveList[j] = ln.ToNurbsCurve() as Curve;

                        }
                    }

                    Brep [] b = Brep.CreatePlanarBreps(curveList, tol);

                
                    faces[i] = b[0];
                }



            }

        
            Brep[] solidRhinoBrep = Brep.CreateSolid(faces, tol);


         
            return solidRhinoBrep[0];
        }

        #endregion


    }
}


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino;
using Rhino.Geometry;

using ParasiteIO.Core.Exceptions;
using Rhino.Collections;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using ParasiteIO.Core.Sync;
using ParasiteIO.Core.Types.Geometry;
using Grasshopper;
using Grasshopper.Kernel.Data;

namespace ParasiteIO.Conversion.Rhinoceros
{
   


    public class RhinoConversion
    {
      

     


        #region POINTS

        public static Point3d ToRhinoType(Parasite_Point3d p) => new Point3d(p.X, p.Y, p.Z);

        public static Point3d ToRhinoType(Autodesk.DesignScript.Geometry.Point p) => new Point3d(p.X, p.Y, p.Z);

        public static IEnumerable<Point3d> ToRhinoType(IEnumerable<Parasite_Point3d> points)
        {
            return points.Select(a => ToRhinoType(a)).ToArray();
        }


        public static IEnumerable<Point3d> ToRhinoType(IEnumerable<Autodesk.DesignScript.Geometry.Point> points)
        {
            return points.Select(a => ToRhinoType(a)).ToArray();
        }

        #endregion

        #region MESH

        public static Mesh ToRhinoType(Parasite_Mesh pMesh)
        {
            int[][] faceIndexes = pMesh.FaceIndexes; 

            Point3d [] vertices =  ToRhinoType(pMesh.Vertices).ToArray(); 

            Mesh m = new Mesh();        

            for (int i = 0; i < faceIndexes.Length; i++)
            {

                int indexA, indexB, indexC, indexD;

                if (faceIndexes[i].Length == 3)
                {
                     indexA = faceIndexes[i][0];
                     indexB = faceIndexes[i][1];
                     indexC = faceIndexes[i][2];          
                    m.Faces.AddFace(indexA, indexB, indexC);
                }


                if (faceIndexes[i].Length == 4)
                {
                     indexA = faceIndexes[i][0];
                     indexB = faceIndexes[i][1];
                     indexC = faceIndexes[i][2];
                     indexD = faceIndexes[i][3];
                    m.Faces.AddFace(indexA, indexB, indexC, indexD);
                }

                
            }

            m.Vertices.AddVertices(vertices);    

            return m;
        }


        #endregion


        #region CURVES,LINES,POLYLINES/POLYCURVES

        public static IEnumerable<Curve> ToRhinoType (IEnumerable< Parasite_Line> lines)
        {
           return  lines.Select(a => Curve.CreateControlPointCurve(ToRhinoType(a.Vertices), 1)).ToArray();
        }

        public static IEnumerable<Curve> ToRhinoType (IEnumerable<Autodesk.DesignScript.Geometry.Edge> edges)
        {
           return edges.Select(a => Curve.CreateControlPointCurve(
                ToRhinoType(new Autodesk.DesignScript.Geometry.Point[] 
                { a.StartVertex.PointGeometry, a.EndVertex.PointGeometry }), 1)).ToArray();
        }


        #region POLYCURVE



        #endregion


        #endregion

        #region SURFACES



        #endregion

        #region SOLID BREPS

        public static Sphere ToRhinoType(Parasite_Sphere sph) => new Sphere(ToRhinoType(sph.Center), sph.Radius);






        public static Brep ToRhinoType(Autodesk.DesignScript.Geometry.Solid solid, double RhinoDocTol)
        {
            int n = solid.Faces.Length;
            double tol = 0.0001;
            Brep[] faces = new Brep[n];

            for (int i = 0; i < n; i++)
            {
               Point3d [] verts = ToRhinoType(solid.Vertices.Select(a => a.PointGeometry).ToArray()).ToArray();



                if (verts.Length < 3) throw new ParasiteArgumentException("A Brep Face cant be constructed from less than 2 vertices!");

                else if (verts.Length == 3)
                {
                    Point3d a = verts[0];
                    Point3d b = verts[1];
                    Point3d c = verts[2];
                    faces[i] = Brep.CreateFromCornerPoints(a, b, c, RhinoDocTol);
                }

                else if (verts.Length == 4)
                {
                    Point3d a = verts[0];
                    Point3d b = verts[1];
                    Point3d c = verts[2];
                    Point3d d = verts[3];
                    faces[i] = Brep.CreateFromCornerPoints(a, b, c, d, RhinoDocTol);
                }

                /// Build curve from vertices to then build a Brep face
                else
                {
                    

                    IEnumerable<Curve> edgesAsCurves=  ToRhinoType(solid.Faces[i].Edges);

                    Brep[] b = Brep.CreatePlanarBreps(edgesAsCurves, RhinoDocTol);

                    faces[i] = b[0];
                }



            }



            Brep output = null;
            try
            {

                Brep[] joinedBreps = Brep.JoinBreps(faces, tol);

                output = joinedBreps[0];
            }

            catch
            {
                if (output == null)
                {
                    // Check if folder exists, if not create it.
                    if (!Directory.Exists(FolderInfo.dirPath))
                    {
                        Directory.CreateDirectory(FolderInfo.dirPath);
                    }

                    string pathToFolder = Path.Combine(FolderInfo.dirPath, "RebelBreps");

                    FileStream fs = new FileStream(pathToFolder, FileMode.Create);

                    // Construct a BinaryFormatter and use it to serialize the data to the stream.
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        formatter.Serialize(fs, faces);
                    }
                    catch (SerializationException e)
                    {
                        throw new SerializationException("Failed to serialize: " + e.Message);
                    }
                    finally
                    {
                        fs.Close();
                    }

                    throw new ParasiteConversionExceptions(output.GetType(), solid.GetType());
                }


            }




            return output;
        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="solid"></param>
        /// <returns></returns>
        public static Brep ToRhinoType(Parasite_BrepSolid solid, double RhinoDocTol)
        {
            int n = solid.Faces.Length;
           
            Brep[] faces = new Brep[n];

            for (int i = 0; i < n; i++)
            {
                Parasite_Point3d[] verts = solid.Faces[i].Vertices.ToArray();



                if (verts.Length < 3) throw new ParasiteArgumentException("A Brep Face cant be constructed from less than 2 vertices!");

                else if (verts.Length == 3)
                {
                    Point3d a = ToRhinoType(verts[0]);
                    Point3d b = ToRhinoType(verts[1]);
                    Point3d c = ToRhinoType(verts[2]);
                    faces[i] = Brep.CreateFromCornerPoints(a, b, c, RhinoDocTol);
                }

                else if (verts.Length == 4)
                {
                    Point3d a = ToRhinoType(verts[0]);
                    Point3d b = ToRhinoType(verts[1]);
                    Point3d c = ToRhinoType(verts[2]);
                    Point3d d = ToRhinoType(verts[3]);
                    faces[i] = Brep.CreateFromCornerPoints(a, b, c, d, RhinoDocTol);
                }

                /// Build curve from vertices to then build a Brep face
                else
                {
                    IEnumerable<Parasite_Line> edges = solid.Faces[i].Edges;

                    IEnumerable<Curve> edgesAsCurves = ToRhinoType(edges);

                    Brep[] b = Brep.CreatePlanarBreps(edgesAsCurves, RhinoDocTol);

                    faces[i] = b[0];
                }



            }

         
          
            Brep output = null;
            try
            {            
                Brep[] joinedBreps = Brep.JoinBreps(faces, RhinoDocTol);
                output = joinedBreps[0];
            }

            catch
            {
                if(output == null)
                {
                    // Check if folder exists, if not create it.
                    if (!Directory.Exists(FolderInfo.dirPath))
                    {
                        Directory.CreateDirectory(FolderInfo.dirPath);
                    }

                    string pathToFolder = Path.Combine(FolderInfo.dirPath, "RebelBreps");

                    FileStream fs = new FileStream(pathToFolder, FileMode.Create);

                    // Construct a BinaryFormatter and use it to serialize the data to the stream.
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        formatter.Serialize(fs, faces);
                    }
                    catch (SerializationException e)
                    {
                        throw new SerializationException("Failed to serialize: " + e.Message);
                    }
                    finally
                    {
                        fs.Close();
                    }

                    throw new ParasiteConversionExceptions(output.GetType(), solid.GetType());
                }

                
            }

       
           

            return output;
        }

        #endregion


    }
}

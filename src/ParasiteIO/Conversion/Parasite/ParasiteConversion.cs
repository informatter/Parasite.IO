﻿
using System;
using System.Collections.Generic;
using System.Linq;



using SharpMatter.SharpExtensions;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using System.Drawing;

using Autodesk.DesignScript.Geometry;

using DB = Autodesk.Revit.DB;

using ParasiteIO.Conversion.Parasite;
using ParasiteIO.Core.Exceptions;
using ParasiteIO.Core.Types.Geometry;
using ParasiteIO.Core.Data.Parameter;

namespace ParasiteIO.Conversion.Parasite
{

    /// <summary>
    /// This class contains all the necesary methods to convert different 
    /// application data types to parasite types
    /// </summary>
    public partial class ParasiteConversion
    {
        #region POINTS
        public static  Parasite_Point3d ToParasiteType( Point3d pt) => new Parasite_Point3d(pt.X, pt.Y, pt.Z);

        public static Parasite_Point3d ToParasiteType(Autodesk.DesignScript.Geometry.Point pt) => new Parasite_Point3d(pt.X, pt.Y, pt.Z);


        #endregion



        #region MESHES
        public static Parasite_Mesh ToParasiteType( Rhino.Geometry.Mesh mesh, Dictionary<string, Parameter> properties = null)
        {

            if (!mesh.IsValid) throw new ParasiteArgumentException("Please input a valid Rhino Mesh!");

            Rhino.Geometry.Collections.MeshFaceList faces = mesh.Faces;
            int[][] faceIndexes = new int[faces.Count][];  
            Parasite_Point3d[] vertices = mesh.Vertices.ToPoint3dArray().Select(a => ToParasiteType(a)).ToArray();

            Parasite_Mesh parasiteMesh=null;


            for (int i = 0; i < faces.Count; i++)
             {
                if (faces[i].IsTriangle)
                        faceIndexes[i] = new int[] { faces[i].A, faces[i].B, faces[i].C };

                if (faces[i].IsQuad)
                        faceIndexes[i] = new int[] { faces[i].A, faces[i].B, faces[i].C, faces[i].D };
            }

            if (mesh.VertexColors.Count != 0)
            {
                Color[] vertexColors = new Color[mesh.VertexColors.Count];
                for (int i = 0; i < mesh.VertexColors.Count; i++)
                {
                    vertexColors[i] = mesh.VertexColors[i];
                }

                parasiteMesh = new Parasite_Mesh(faceIndexes, vertices, vertexColors, properties);
            }

            if(mesh.VertexColors.Count == 0)  parasiteMesh = new Parasite_Mesh(faceIndexes, vertices,null, properties);


            if (!parasiteMesh.IsValid) throw new ParasiteConversionExceptions(mesh.GetType(), parasiteMesh.GetType());

            return parasiteMesh;
   
        }


        public static Parasite_Mesh ToParasiteType(Autodesk.DesignScript.Geometry.Mesh mesh, Dictionary<string, string> properties = null)
        {

            throw new NotImplementedException();

        }

        #endregion

        #region CURVES/ LINES/ NURBSCURVES / ARCS /

        #region LINES

        public static IEnumerable<Parasite_Line> ToParasiteType( IEnumerable<Autodesk.DesignScript.Geometry.Curve> curves )
        {

            Parasite_Line[] output = new Parasite_Line[curves.Count()];

            for (int i = 0; i < curves.Count(); i++)
            {
                Autodesk.DesignScript.Geometry.Curve crv = curves.ElementAt(i);

                
   
                ////THIS IS FAILING IN DIFFERENT CASES FOR SOME REASON
                //Vector tanVecStrt = crv.ToNurbsCurve().TangentAtParameter(0.25/*crv.StartParameter()*/);
                //Vector tanVecEnd = crv.ToNurbsCurve().TangentAtParameter(0.50/*crv.EndParameter()*/);

                //// TODO  WETHER CURVE IS LINEAR
                //if (tanVecStrt.Dot(tanVecEnd) !=1.0)
                //    throw new ParasiteArgumentException("Cant convert a Dynamo Curve to a Parasite_Line!");

                ////THIS IS FAILING IN DIFFERENT CASES FOR SOME REASON

                output[i] = new Parasite_Line(ToParasiteType(crv.StartPoint), ToParasiteType(crv.EndPoint), null);
            }

            return output;
        }

        #endregion



        public static Parasite_NurbsCurve ToParasiteType(Rhino.Geometry.NurbsCurve nurbsCurve, Dictionary<string, string> properties = null)
        {
            

            Parasite_Point3d [] controlPoints = nurbsCurve.Points.Select(a=>ToParasiteType(a.Location)).ToArray();
            double[] weights = nurbsCurve.Points.Select(a => a.Weight).ToArray();

            //making sure all the weights are not zeros by setting them to 1 if they are
            double tolerance = 1e-4;
            if (weights.Any((x) => x <= tolerance))
                weights = weights.Select((x) => 1.0).ToArray(); 

            double[] knots = nurbsCurve.Knots.Select(a => a).ToArray();
            double[] interiorKnotMultiplicity = new double[nurbsCurve.Knots.Count];

            for (int i = 0; i < interiorKnotMultiplicity.Length; i++)
            {
                interiorKnotMultiplicity[i] = nurbsCurve.Knots.KnotMultiplicity(i);
            }

            return new Parasite_NurbsCurve(controlPoints, weights, knots, interiorKnotMultiplicity, nurbsCurve.Degree);

        }

        #endregion

        #region BREPS

        #region BREPS - SURFACE

        public static Parasite_BrepSurface ToParasiteType(Rhino.Geometry.Brep brep, Dictionary<string, Parameter> properties = null)
        {
           
            if (!brep.IsValid)
                throw new ParasiteArgumentException("Please input a valid Rhino Brep!");

            List<Parasite_Point3d> vertices = brep.Vertices.Select(x => x.Location).Where(y => y.IsValid).Select(z => ToParasiteType(z)).ToList();


            return new Parasite_BrepSurface(vertices,null,properties); 
        }

        #endregion

        #region BREPS - SOLID


        /// <summary>
        /// Converts a Dynamo Solid to a Parasite Brep Solid.
        /// This method iterates through all the faces of the Dynamo Solid,
        /// extracts their vertices and computes a new Parasite Brep Surface from them.
        /// Each Parasite Brep Surface is sequentially added to a Collection as input for constructing 
        /// a Parasite Brep Solid
        /// </summary>
        /// <param name="solid"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static Parasite_BrepSolid ToParasiteType(Autodesk.DesignScript.Geometry.Solid solid, Dictionary<string, Parameter> properties = null)
        {
            Autodesk.DesignScript.Geometry.Face[] faces = solid.Faces;

            Parasite_BrepSurface[] brepSurfs = new Parasite_BrepSurface[faces.Length];

            for (int i = 0; i < faces.Length; i++)
            {
                Autodesk.DesignScript.Geometry.Loop [] loops = faces[i].Loops;

                List<Autodesk.DesignScript.Geometry.Curve> lines = new List<Autodesk.DesignScript.Geometry.Curve>();

                for (int k = 0; k < loops.Length; k++)
                {
                    Autodesk.DesignScript.Geometry.CoEdge[] coEdge = loops[k].CoEdges;


                    for (int m = 0; m < coEdge.Length; m++)
                    {
                        lines.Add(coEdge[m].Edge.CurveGeometry);
                    }

                }


                Autodesk.DesignScript.Geometry.Vertex[] vertices = faces[i].Vertices;

                Parasite_Point3d[] vertexArray = new Parasite_Point3d[vertices.Length];

                for (int j = 0; j < vertices.Length; j++)               
                    vertexArray[j] = ToParasiteType(vertices[j].PointGeometry);
                

                Parasite_BrepSurface brepSrf = new Parasite_BrepSurface(vertexArray, ToParasiteType(lines), properties);
                brepSurfs[i] = brepSrf;

            }

            return new Parasite_BrepSolid(brepSurfs, properties);


        }


        //public static Parasite_BrepSurface ToParasiteType(this DB.Face face, bool untrimmed = false)
        //{
        //    var surface = face.ToRhinoSurface();
        //    if (surface is null)
        //        return null;

        //    var brep = Brep.CreateFromSurface(surface);
        //    if (brep is null)
        //        return null;


        //    if (untrimmed)
        //        return brep;

        //    var loops = face.GetEdgesAsCurveLoops().ToRhino().ToArray();



        //    try { return brep.TrimFaces(loops); }
        //    finally { brep.Dispose(); }
        //}


        //public static Parasite_BrepSolid ToParasiteType(DB.Solid solid)
        //{
        //    return solid.Faces.
        //           Cast<DB.Face>().
        //           Select(x => x.ToRhino()).
        //           ToArray().
        //           JoinAndMerge(Revit.VertexTolerance);
        //}


        #endregion


        #region NURBS SURFACE

        public static Parasite_NurbsSurface ToParasiteType (Rhino.Geometry.NurbsSurface nurbsSurface, Dictionary<string, string> properties = null)
        {
            Parasite_NurbsSurface parasite_NurbsSurface = null;

            if (!nurbsSurface.IsValid) throw new ParasiteArgumentException("Please enter a valid Rhino Nurbs Surface");

            else
            {
                if (nurbsSurface.IsSphere())
                    throw new ParasiteNotImplementedExceptions("There is still no support for Rhino NURBS Spheres");

                else if (nurbsSurface.IsTorus())
                    throw new ParasiteNotImplementedExceptions("There is still no support for Rhino NURBS Torus");

                else
                {
                    NurbsSurfaceKnotList rhinoKnotsU = nurbsSurface.KnotsU;
                    NurbsSurfaceKnotList rhinoKnotsV = nurbsSurface.KnotsV;
                    NurbsSurfacePointList cp = nurbsSurface.Points;

                    double[] knotsU = new double[rhinoKnotsU.Count];
                    double[] knotsV = new double[rhinoKnotsV.Count];
                    double[][] weights = new double[cp.Count()][];

                    if (cp.Any((x) => !x.Location.IsValid)) throw new ParasiteArgumentException("The Rhino NURBS Surface had an invalid Control Point!");

                    Parasite_Point3d[][] vertices = new Parasite_Point3d[cp.Count()][];

                    for (int i = 0; i < rhinoKnotsU.Count; i++)
                    {
                        knotsU[i] = rhinoKnotsU[i];
                        knotsV[i] = rhinoKnotsV[i];
                    }


                    //for (int u = 0; u < cp.CountU; u++)
                    //{
                    //    for (int v = 0; v < cp.CountV; v++)
                    //    {
                    //        Point3d controlP = cp.GetControlPoint(u, v).Location;
                    //        double weight = cp.GetWeight(u, v);

                    //    }
                    //}


                


                    int count = -1;
                    foreach (var item in cp)
                    {

                        count++;
                        if (item.Weight <= 1e-11)
                            weights[count] = new double[] { 0.0 };

                        weights[count] = new double[] { item.Weight };
                        vertices[count] = new Parasite_Point3d[] { ToParasiteType(item.Location) };

                    }

                    parasite_NurbsSurface = new Parasite_NurbsSurface(vertices, knotsU, knotsV, weights, nurbsSurface.Degree(0), nurbsSurface.Degree(1));

                }
            }

            return parasite_NurbsSurface;
        }

        #endregion

        #endregion
    }
}

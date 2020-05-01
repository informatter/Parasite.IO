
using System;
using System.Collections.Generic;
using System.Linq;
using Rhino;

using Autodesk.DesignScript.Geometry;
using ParasiteIO.Conversion.Dynamo;
using ParasiteIO.Core.Types.Geometry;
using ParasiteIO.Core.Types;
using ParasiteIO.Core.Exceptions;
//using Rhino.Geometry;

namespace ParasiteIO.Conversion.Dynamo
{
    /// <summary>
    /// This class contains methods to convert from Parasite data types to
    /// Dynamo data types
    /// </summary>
    public partial class DynamoConversion
    {

        #region POINTS
        /// <summary>
        /// Converts a Parasite Point3d to a Dynamo Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static Autodesk.DesignScript.Geometry.Point ToDynamoType(Parasite_Point3d pt) =>
            Autodesk.DesignScript.Geometry.Point.ByCoordinates(pt.X, pt.Y, pt.Z);


        /// <summary>
        /// Converts a collection of Parasite Point3d's to a collection of Dynamo Points
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static IEnumerable<Autodesk.DesignScript.Geometry.Point> ToDynamoType(IEnumerable<Parasite_Point3d> points) =>
            points.Select(p => ToDynamoType(p));

        public static Autodesk.DesignScript.Geometry.Point [][] ToDynamoType(Parasite_Point3d[][] points)
        {
            Point[][] dPts = new Point[points.Length][];
            for (int i = 0; i < points.Length; i++)
            {
                for (int j = 0; j < points[i].Length; i++)
                {
                    dPts[i] = new Point[] { ToDynamoType(points[i][j]) };
                }
            }

            return dPts;
        }

        #endregion

        #region MESH
        /// <summary>
        /// Converts a Parasite Mesh to a Dynamo Mesh
        /// </summary>
        /// <param name="mesh">Parasite mesh object </param>
        /// <returns>A Dynamo Mesh object</returns>
        public static Autodesk.DesignScript.Geometry.Mesh ToDynamoType(Parasite_Mesh mesh)
        {
            int[][] faceIndexes = mesh.FaceIndexes;
            Parasite_Point3d[] verticesTemp = mesh.Vertices;

            List<IndexGroup> indexGroups = new List<IndexGroup>();
            List<Autodesk.DesignScript.Geometry.Point> vertices = verticesTemp.Select(x => ToDynamoType(x)).ToList();

            for (int i = 0; i < faceIndexes.Length; i++)
            {
                for (int j = 0; j < faceIndexes[i].Length; j++)
                {
                    if (faceIndexes[i].Length == 3)
                    {
                        IndexGroup ig = IndexGroup.ByIndices((uint)faceIndexes[i][0], 
                            (uint)faceIndexes[i][1], (uint)faceIndexes[i][2]);
                        indexGroups.Add(ig);
                    }

                    if (faceIndexes[i].Length == 4)
                    {
                        IndexGroup ig = IndexGroup.ByIndices((uint)faceIndexes[i][0], 
                            (uint)faceIndexes[i][1], (uint)faceIndexes[i][2], (uint)faceIndexes[i][3]);
                        indexGroups.Add(ig);
                    }

                    if (faceIndexes[i].Length > 4 || faceIndexes[i].Length < 3)
                        throw new ParasiteArgumentException("A Dynamo mesh cant have a face with more than 4 vertices or less than 3");

                }
            }



            Autodesk.DesignScript.Geometry.Mesh dMesh = Autodesk.DesignScript.Geometry.Mesh.ByPointsFaceIndices(vertices, indexGroups);

            // TODO: Check for mesh validity

            return dMesh;
        }

        #endregion


        #region CURVES,LINES,POLYLINES/POLYCURVES


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nurbsC"></param>
        /// <returns></returns>
        public static Autodesk.DesignScript.Geometry.NurbsCurve ToDynamoType (Parasite_NurbsCurve nurbsC)
        {
           

            Autodesk.DesignScript.Geometry.Point[] vertices = nurbsC.ControlPoints.Select(a=>ToDynamoType(a)).ToArray();
            double[] weights = nurbsC.Weights;
            double[] knots = nurbsC.Knots;
            double[] interiorKnotMult_Temp = nurbsC.InteriorKnotMultiplicity;
            int degree = nurbsC.Degree;

            double[] interiorKnotMult = new double[knots.Length];

            for (int i = 0; i < interiorKnotMult_Temp.Length; i++)
            {
                if(interiorKnotMult_Temp[i] > degree +1)
                    interiorKnotMult[i] = degree + 1;

                interiorKnotMult[i] = interiorKnotMult[i];

            }


            // NurbsCurve acscs =  Autodesk.DesignScript.Geometry.NurbsCurve.ByControlPointsWeightsKnots(vertices, weights, knots, degree);



            //return Autodesk.DesignScript.Geometry.NurbsCurve.ByControlPointsWeightsKnots(vertices, weights, knots, degree);

            return Autodesk.DesignScript.Geometry.NurbsCurve.ByControlPoints(vertices, degree);


        }
         


        #region POLYCURVE

        /// <summary>
        /// Converts a Parasite Polyline to a Dynamo Polyline
        /// </summary>
        /// <param name="pl"></param>
        /// <returns></returns>
        public static Autodesk.DesignScript.Geometry.PolyCurve ToDynamoType( Parasite_Polyline pl)
        {
            Autodesk.DesignScript.Geometry.PolyCurve polyCurve = null;
            if (pl.Closed)
                polyCurve = Autodesk.DesignScript.Geometry.PolyCurve.ByPoints(pl.Vertices.Select(a => ToDynamoType(a)).ToArray(), true);

            else
                polyCurve = Autodesk.DesignScript.Geometry.PolyCurve.ByPoints(pl.Vertices.Select(a => ToDynamoType(a)).ToArray(), false);


            return polyCurve;
        }

        #endregion


        #endregion

        #region SURFACES

        /// <summary>
        /// Converts a Parasite_BrepSurface object to a Dynamo Surface object
        /// </summary>
        /// <param name="brep">A Parasite_BrepSurface object </param>
        /// <returns>A Dynamo Surface on success</returns>
        public static Autodesk.DesignScript.Geometry.Surface ToDynamoType( Parasite_BrepSurface brep)
        {
            IEnumerable<Autodesk.DesignScript.Geometry.Point> vertices = brep.Vertices.Select(x => ToDynamoType(x));
            return Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(vertices);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nurbsSurface"></param>
        /// <returns></returns>
        /// 

            [Obsolete("Method needs to improve, returning wrong results")]
        public static Autodesk.DesignScript.Geometry.NurbsSurface ToDynamoType( Parasite_NurbsSurface nurbsSurface)
        {
            Parasite_Point3d[][] vertices = nurbsSurface.ControlPoints;
            double[][] weights = nurbsSurface.Weights;
            double[] knotsU = nurbsSurface.KnotsU;
            double[] knotsV = nurbsSurface.KnotsV;

            return Autodesk.DesignScript.Geometry.NurbsSurface.ByControlPointsWeightsKnots(ToDynamoType(vertices), weights, knotsU, knotsV,nurbsSurface.DegreeU,nurbsSurface.DegreeV);

            //return Autodesk.DesignScript.Geometry.NurbsSurface.ByControlPoints(vertices.ToDynamoType(), nurbsSurface.DegreeU, nurbsSurface.DegreeV);
        }


        #endregion

        #region SOLID BREPS


        /// <summary>
        /// 
        /// </summary>
        /// <param name="brep"></param>
        /// <returns></returns>
        public static Autodesk.DesignScript.Geometry.Solid ToDynamoType( Parasite_BrepSolid brep)
        {
            //Parasite_Point3d[][] vertices = brep.Vertices;

            //List<Autodesk.DesignScript.Geometry.Surface> surfaces = new List<Autodesk.DesignScript.Geometry.Surface>();

            //for (int i = 0; i < vertices.Length; i++)
            //{
            //    IEnumerable<Autodesk.DesignScript.Geometry.Point> pts = vertices[i].Select(x => ToDynamoType(x));
            //    surfaces.Add(Autodesk.DesignScript.Geometry.Surface.ByPerimeterPoints(pts));
            //}


            //return Autodesk.DesignScript.Geometry.Solid.ByJoinedSurfaces(surfaces);

            throw new NotImplementedException();
        }

        #endregion


    }
}

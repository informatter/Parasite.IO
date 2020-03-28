using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ParasiteIO.Core.Types.Geometry;

using Autodesk.DesignScript.Geometry;

using Rhino.Geometry;
using Rhino.Geometry.Collections;

namespace ParasiteIO.Conversion.Dynamo
{
    public static  class ToDynamoExtensions
    {
     


        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Autodesk.DesignScript.Geometry.Point [][]  ToDynamoType(this double [][] points)
        {
            Autodesk.DesignScript.Geometry.Point[][] vertices = new Autodesk.DesignScript.Geometry.Point[points.Length][];

            for (int i = 0; i < points.Length; i++)
            {
                vertices[i] = new Autodesk.DesignScript.Geometry.Point[] {Autodesk.DesignScript.Geometry.Point.ByCoordinates(points[i][0], points[i][1], points[i][2]) };
            }

            return vertices;
        }




     




    }
}

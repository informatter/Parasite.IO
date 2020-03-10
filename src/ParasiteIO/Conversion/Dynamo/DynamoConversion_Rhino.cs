using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Core.Conversion.Dynamo
{
    public abstract class DynamoConversion
    {

        /// <summary>
        /// Converts a collection of Rhino Point3d's to a Collection of Dynamo Points
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static IEnumerable<Autodesk.DesignScript.Geometry.Point> ToDynamoType(IEnumerable<Point3d> points) =>
            points.Select(p => ToDynamoType(p));


        /// <summary>
        /// Converts a Rhino Point3d to a Dynamo Point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static Autodesk.DesignScript.Geometry.Point ToDynamoType(Point3d pt) =>
             Autodesk.DesignScript.Geometry.Point.ByCoordinates(pt.X, pt.Y, pt.Z);

    }
}

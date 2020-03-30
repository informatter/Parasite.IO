
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DynamoSolid = Autodesk.DesignScript.Geometry.Solid;
using Autodesk.Revit.DB;
using System.Diagnostics;
using Revit.GeometryConversion;

namespace ParasiteIO.Dynamo
{
    internal class GeometryUtility
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="geoObject"></param>
        /// <param name="outPutSolids"></param>
        public static void SolidFromGeometryObject(GeometryObject geoObject, List<DynamoSolid> outPutSolids)
        {
            // Check if its is a Geometry Instance
            if (geoObject is GeometryInstance)
            {
                GeometryInstance geoInstance = geoObject as GeometryInstance;

                foreach (Solid geo in geoInstance.GetInstanceGeometry())
                {
                    // Skip to next element in iteration if any of the following conditions are met                                   
                    if (null == geo || 0 == geo.Faces.Size || 0 == geo.Edges.Size)
                        continue;

                    DynamoSolid solidGeo = ToDynamoSolid(geo);

                    // Sometimes a geometry element can be a null solid
                    if (solidGeo != null)
                        outPutSolids.Add(solidGeo);

                }

            }

            // Check if it is Solid
            else if (geoObject is Solid)
            {
                Solid solid = geoObject as Solid;
                DynamoSolid solidGeo = ToDynamoSolid(solid);

                // Sometimes a geometry element can be a null solid
                if (solidGeo != null)
                    outPutSolids.Add(solidGeo);

            }
        }


        /// <summary>
        /// Wrapper for ToProtoType() method so the debugger
        /// does not step in the method, but still executes it
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [DebuggerStepThroughAttribute]
        public static DynamoSolid ToDynamoSolid(Solid s) => s.ToProtoType();

    }
}

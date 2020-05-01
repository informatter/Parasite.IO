using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using ParasiteIO.Core.Data.Properties;

using ParasiteIO.Core.Types;

using DynSolid = Autodesk.DesignScript.Geometry.Solid;
using RvtSolid = Autodesk.Revit.DB.Solid;

using P_Parameter = ParasiteIO.Core.Data.Parameter.Parameter;
using P_StorageType = ParasiteIO.Core.Data.Parameter.StorageType;
using P_ParameterType = ParasiteIO.Core.Data.Parameter.ParameterType;
using Autodesk.Revit.DB.Structure;
using ParasiteIO.Core.Exceptions;

namespace ParasiteIO.Dynamo.Core
{
    internal class DynamoParasiteFactory
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="parasiteObject"></param>
        public static void AddParametersToParasiteObj(ParasiteObject parasiteObject, P_Parameter[] parameters=null)
        {
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                    parasiteObject.AddParameter(parameters[i].ParameterType.ToString(), parameters[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="dynSolid"></param>
        /// <returns></returns>
        public static ParasiteObject CreateParasiteObject( DynSolid dynSolid, P_Parameter[] parameters = null)
        {
            ParasiteObject parasiteObject = new ParasiteObject(dynSolid);

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                    parasiteObject.AddParameter(parameters[i].ParameterType.ToString(), parameters[i]);
            }

            return parasiteObject;
        }



        public static ParasiteObject CreateParasiteObject(DynSolid dynSolid)
        {
            ParasiteObject parasiteObject = new ParasiteObject(dynSolid);
            return parasiteObject;
        }


       /// <summary>
       /// 
       /// </summary>
       /// <param name="geometryElement"></param>
       /// <param name="parameters"></param>
       /// <param name="outPutSolids"></param>
       /// <param name="parasiteObjects"></param>
        public static void CreateParasiteObjectFromGeometryElement(GeometryElement geometryElement, P_Parameter [] parameters,
            List<DynSolid> outPutSolids, List<ParasiteObject> parasiteObjects )
        {
            bool success;
            ParasiteObject parasiteObject = new ParasiteObject();
            foreach (GeometryObject item in geometryElement)
            {
                success = GeometryExtraction.TryGetGeometryFromGeometryObject(item, out Autodesk.DesignScript.Geometry.Solid dynS);

                if(success)
                {
                    parasiteObject = CreateParasiteObject(dynS);
                    AddParametersToParasiteObj(parasiteObject, parameters);
                    parasiteObjects.Add(parasiteObject);
                    outPutSolids.Add(dynS);
                }

            }
           
        }


    }
}

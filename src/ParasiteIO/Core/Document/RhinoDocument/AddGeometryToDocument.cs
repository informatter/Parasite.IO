using ParasiteIO.Conversion.Rhinoceros;
using ParasiteIO.Core.Exceptions;
using ParasiteIO.Core.Types;
using ParasiteIO.Core.Types.Geometry;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParasiteIO.Core.Document.RhinoDocument
{
    public class AddGeometryToDocument
    {

        #region METHODS

        /// <summary>
        /// Adds the geometry contained in each ParasiteObject to the currrent Rhino Document
        /// </summary>
        /// <param name="parasiteObjects"></param>
        /// <param name="objectAttributes"></param>
        public static void AddToDocument(List<ParasiteObject> parasiteObjects, List<ObjectAttributes> objectAttributes, Rhino.RhinoDoc DOC)
        {
            // Not thread safe
            for (int i = 0; i < parasiteObjects.Count; i++)
                Bake(parasiteObjects[i], objectAttributes[i], DOC);
        }



        /// <summary>
        /// "Bakes" geometry to the current Rhino Document
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="att"></param>
        private static void Bake(ParasiteAbstractObject obj, ObjectAttributes att , Rhino.RhinoDoc DOC)
        {
            if (obj == null) 
                throw new ParasiteArgumentException("You are attempting to bake a null object!");

            if(obj is ParasiteObject)
            {
                ParasiteObject parasiteObject = obj as ParasiteObject;

                if (parasiteObject.Data is Parasite_BrepSolid solid)
                {

                    DOC.Objects.AddBrep(RhinoConversion.ToRhinoType(solid, 0.001), att);

                }

                else
                    throw new ParasiteNotImplementedExceptions($"Baking capabilities for " +
                        $"{parasiteObject.Data.GetType().ToString()} are still not implemented! check back later");
              
            }
          
        }


        //public static void Bake(object obj, ObjectAttributes att)
        //{
        //    if (obj == null)
        //        return;

        //    //Bake to the right type of object
        //    if (obj is GeometryBase)
        //    {
        //        GeometryBase geomObj = obj as GeometryBase;

        //        switch (geomObj.ObjectType)
        //        {
        //            case Rhino.DocObjects.ObjectType.Brep:
        //                DOC.Objects.AddBrep(obj as Brep, att);
        //                break;
        //            case Rhino.DocObjects.ObjectType.Curve:
        //                DOC.Objects.AddCurve(obj as Curve, att);
        //                break;
        //            case Rhino.DocObjects.ObjectType.Point:
        //                DOC.Objects.AddPoint((obj as Point).Location, att);
        //                break;
        //            case Rhino.DocObjects.ObjectType.Surface:
        //                DOC.Objects.AddSurface(obj as Surface, att);
        //                break;
        //            case Rhino.DocObjects.ObjectType.Mesh:
        //                DOC.Objects.AddMesh(obj as Mesh, att);
        //                break;

        //            default:
        //                //Print("The script does not know how to handle this type of geometry: " + obj.GetType().FullName);
        //                return;
        //        }
        //    }

        //    else
        //    {
        //        Type objectType = obj.GetType();

        //        if (objectType == typeof(Arc))
        //        {
        //            DOC.Objects.AddArc((Arc)obj, att);
        //        }
        //        else if (objectType == typeof(Box))
        //        {
        //            DOC.Objects.AddBrep(((Box)obj).ToBrep(), att);
        //        }
        //        else if (objectType == typeof(Circle))
        //        {
        //            DOC.Objects.AddCircle((Circle)obj, att);
        //        }
        //        else if (objectType == typeof(Ellipse))
        //        {
        //            DOC.Objects.AddEllipse((Ellipse)obj, att);
        //        }
        //        else if (objectType == typeof(Polyline))
        //        {
        //            DOC.Objects.AddPolyline((Polyline)obj, att);
        //        }
        //        else if (objectType == typeof(Sphere))
        //        {
        //            DOC.Objects.AddSphere((Sphere)obj, att);
        //        }
        //        else if (objectType == typeof(Point3d))
        //        {
        //            DOC.Objects.AddPoint((Point3d)obj, att);
        //        }
        //        else if (objectType == typeof(Line))
        //        {
        //            DOC.Objects.AddLine((Line)obj, att);
        //        }
        //        else if (objectType == typeof(Vector3d))
        //        {
        //            // Print("Impossible to bake vectors");
        //            return;
        //        }
        //    }

        //}

        #endregion
    }
}

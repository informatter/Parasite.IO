using ParasiteIO.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParasiteIO.Core.Exceptions;
using ParasiteIO.Conversion.Rhinoceros;
using Rhino.Geometry;
using ParasiteIO.Core.Types.Geometry;

namespace ParasiteIO.Utilities
{
    public class BakeToLayer
    {
        private static readonly Rhino.RhinoDoc DOC = Rhino.RhinoDoc.ActiveDoc;
        private static readonly Random ran = new Random();
        public static void BakeTo(List<ParasiteObject> parasiteObjects)
        {
            for (int i = 0; i < parasiteObjects.Count; i++)
            {
                string layerName = parasiteObjects[i].Property.Name;

                if (!Rhino.DocObjects.Layer.IsValidName(layerName))
                {
                    throw new ParasiteArgumentException(layerName + " " + " is not a valid layer name");
                }

                if(string.IsNullOrEmpty(layerName))
                {
                    throw new ParasiteArgumentException(layerName + " " + " cant be blank");
                }

                //// Does a layer with the same name already exist?
                //int layer_index = DOC.Layers.Find(layerName, true);
                //if (layer_index >= 0)
                //{
                //    throw new ParasiteArgumentException(string.Format("A layer with the name {0} already exists.", layerName));
                    
                //}

                //// Add a new layer to the DOCument
                //layer_index = DOC.Layers.Add(layerName, System.Drawing.Color.FromArgb(ran.Next(0,255), ran.Next(0, 255), ran.Next(0, 255)));
                //if (layer_index < 0)
                //{
                //    throw new ParasiteArgumentException(string.Format("Unable to add {0} layer.", layerName));
           
                //}

                //Make new attribute to set name
                Rhino.DocObjects.ObjectAttributes att = new Rhino.DocObjects.ObjectAttributes();

                //Set layer
                if (!string.IsNullOrEmpty(layerName) && Rhino.DocObjects.Layer.IsValidName(layerName))
                {
                    //Get the current layer index
                    Rhino.DocObjects.Tables.LayerTable layerTable = DOC.Layers;
                    int layerIndex = layerTable.Find(layerName, true);

                    if (layerIndex < 0) //This layer does not exist, we add it
                    {
                        Rhino.DocObjects.Layer onlayer = new Rhino.DocObjects.Layer
                        {
                            Name = layerName,
                            Color = System.Drawing.Color.FromArgb(ran.Next(0, 255), ran.Next(0, 255), ran.Next(0, 255))
                        }; //Make a new layer

                        layerIndex = layerTable.Add(onlayer); //Add the layer to the layer table
                        if (layerIndex > -1) //We manged to add layer!
                        {
                            att.LayerIndex = layerIndex;
            

                        }
                         else
                            throw new ParasiteArgumentException(string.Format("Unable to add {0} layer.", layerName));
                    }
                    else
                        att.LayerIndex = layerIndex; //We simply add to the existing layer
                }


                if (parasiteObjects[i].Data is Parasite_BrepSolid solid)
                {

                     DOC.Objects.AddBrep(RhinoConversion.ToRhinoType(solid, 0.001),att);
                
                }

     
            }

            //for (int i = 0; i < parasiteObjects.Count; i++)
            //{
            //    if(parasiteObjects[i].Data is Autodesk.DesignScript.Geometry.Solid solid)
            //    {
                   
            //        var addedGeo = DOC.Objects.Add(RhinoConversion.ToRhinoType(solid, 0.001));

            //       // DOC.Objects.AddRhinoObject()
            //    }
               
            //}
        }



        void Bake(object obj, Rhino.DocObjects.ObjectAttributes att)
        {
            if (obj == null)
                return;



            //Bake to the right type of object
            if (obj is GeometryBase)
            {
                GeometryBase geomObj = obj as GeometryBase;

                switch (geomObj.ObjectType)
                {
                    case Rhino.DocObjects.ObjectType.Brep:
                        DOC.Objects.AddBrep(obj as Brep, att);
                        break;
                    case Rhino.DocObjects.ObjectType.Curve:
                        DOC.Objects.AddCurve(obj as Curve, att);
                        break;
                    case Rhino.DocObjects.ObjectType.Point:
                        DOC.Objects.AddPoint((obj as Rhino.Geometry.Point).Location, att);
                        break;
                    case Rhino.DocObjects.ObjectType.Surface:
                        DOC.Objects.AddSurface(obj as Surface, att);
                        break;
                    case Rhino.DocObjects.ObjectType.Mesh:
                        DOC.Objects.AddMesh(obj as Mesh, att);
                        break;

                    default:
                        //Print("The script does not know how to handle this type of geometry: " + obj.GetType().FullName);
                        return;
                }
            }
            else
            {
                Type objectType = obj.GetType();

                if (objectType == typeof(Arc))
                {
                    DOC.Objects.AddArc((Arc)obj, att);
                }
                else if (objectType == typeof(Box))
                {
                    DOC.Objects.AddBrep(((Box)obj).ToBrep(), att);
                }
                else if (objectType == typeof(Circle))
                {
                    DOC.Objects.AddCircle((Circle)obj, att);
                }
                else if (objectType == typeof(Ellipse))
                {
                    DOC.Objects.AddEllipse((Ellipse)obj, att);
                }
                else if (objectType == typeof(Polyline))
                {
                    DOC.Objects.AddPolyline((Polyline)obj, att);
                }
                else if (objectType == typeof(Sphere))
                {
                    DOC.Objects.AddSphere((Sphere)obj, att);
                }
                else if (objectType == typeof(Point3d))
                {
                    DOC.Objects.AddPoint((Point3d)obj, att);
                }
                else if (objectType == typeof(Line))
                {
                    DOC.Objects.AddLine((Line)obj, att);
                }
                else if (objectType == typeof(Vector3d))
                {
                    // Print("Impossible to bake vectors");
                    return;
                }
            }
          
        }
    }
}

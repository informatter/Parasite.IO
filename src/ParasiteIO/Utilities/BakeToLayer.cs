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
using System.Runtime.CompilerServices;
using ParasiteIO.Core.Data.Parameter;
using GH_IO.Serialization;

namespace ParasiteIO.Utilities
{
    public class BakeToLayer
    {
        private static readonly Rhino.RhinoDoc DOC = Rhino.RhinoDoc.ActiveDoc;
        private static readonly Random ran = new Random();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentLayerName"></param>
        /// <param name="parentLayerIndex"></param>
        /// <param name="childLayerName"></param>
        /// <returns></returns>
        public static int AddChildLayer( string parentLayerName,int parentLayerIndex, string childLayerName)
        {
           

            if (!Rhino.DocObjects.Layer.IsValidName(childLayerName))
                throw new ParasiteArgumentException($"{childLayerName} is not a valid layer name"); 

            if (string.IsNullOrEmpty(childLayerName))
                throw new ParasiteArgumentException($"{childLayerName} cant be blank");


            
            if (parentLayerIndex < 0)
                throw new ParasiteArgumentException($"The parent layer specified:{parentLayerName} does not exists in the current Rhino Document");

            Rhino.DocObjects.Layer parent_layer = DOC.Layers[parentLayerIndex];
            

            int childLayerIndex = DOC.Layers.Find(childLayerName, true);
 
            if (childLayerIndex < 0) //This layer does not exist, we add it
            {
                Rhino.DocObjects.Tables.LayerTable layerTable = DOC.Layers;

                int R = parent_layer.Color.R;
                int G = parent_layer.Color.G;
                int B = parent_layer.Color.B;
                Rhino.DocObjects.Layer childlayer = new Rhino.DocObjects.Layer
                {
                    ParentLayerId = parent_layer.Id,
                    Name = childLayerName,
                    
                    Color = System.Drawing.Color.FromArgb(ran.Next(R, 255), ran.Next(G, 255), ran.Next(B, 255))
                };

                childLayerIndex = layerTable.Add(childlayer); //Add the layer to the layer table

                if (childLayerIndex < 0)
                    throw new ParasiteArgumentException(string.Format("Unable to add {0} layer.", parentLayerName));
            }

 

            return childLayerIndex;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parasiteObjects"></param>
        //private static void CreateLayer(List<ParasiteObject> parasiteObjects)
        //{
        //    for (int i = 0; i < parasiteObjects.Count; i++)
        //    {
        //        //Make new attribute to set name
        //        Rhino.DocObjects.ObjectAttributes att = new Rhino.DocObjects.ObjectAttributes();

        //        string parentLayerName=null;
        //        if (parasiteObjects[i].GetParameter(ParameterType.Layer.ToString(), out Parameter value))
        //         {
        //            parentLayerName = value.GetValue();

        //            if (!Rhino.DocObjects.Layer.IsValidName(parentLayerName))
        //                throw new ParasiteArgumentException($"{parentLayerName} is not a valid layer name");

        //            if (string.IsNullOrEmpty(parentLayerName))
        //                throw new ParasiteArgumentException($"{parentLayerName} cant be blank");
        //        }

        //        if (parentLayerName == null)
        //            throw new ParasiteArgumentException($"Could not retrieve Parameter of type {ParameterType.Layer} ");


        //        //Set layer
        //        if (!string.IsNullOrEmpty(parentLayerName) && Rhino.DocObjects.Layer.IsValidName(parentLayerName))
        //        {
        //            //Get the current layer index
        //            Rhino.DocObjects.Tables.LayerTable layerTable = DOC.Layers;
        //            int layerIndex = layerTable.Find(parentLayerName, true);

        //            if (layerIndex < 0) //This layer does not exist, we add it
        //            {

        //                Rhino.DocObjects.Layer onlayer = new Rhino.DocObjects.Layer
        //                {

        //                    Name = parentLayerName,
        //                    Color = System.Drawing.Color.FromArgb(ran.Next(0, 255), ran.Next(0, 255), ran.Next(0, 255))
        //                }; //Make a new layer

        //                layerIndex = layerTable.Add(onlayer); //Add the layer to the layer table

        //                if (layerIndex > -1) //We manged to add layer!
        //                {
        //                    att.LayerIndex = layerIndex;

        //                }
        //                else
        //                    throw new ParasiteArgumentException(string.Format("Unable to add {0} layer.", parentLayerName));
        //            }
        //            else
        //                att.LayerIndex = layerIndex;
        //        }
        //    }
        //}




        public static void BakeTo(List<ParasiteObject> parasiteObjects)
        {
            //Make new attribute to set name
           // Rhino.DocObjects.ObjectAttributes att = new Rhino.DocObjects.ObjectAttributes();
            for (int i = 0; i < parasiteObjects.Count; i++)
            {
                //Make new attribute to set name
                Rhino.DocObjects.ObjectAttributes att = new Rhino.DocObjects.ObjectAttributes();


                string parentLayerName = null;
                string childLayerName = null;
                if (parasiteObjects[i].GetParameter(ParameterType.Layer.ToString(), out Parameter value))
                {
                    parentLayerName = value.GetValue();

                    if (!Rhino.DocObjects.Layer.IsValidName(parentLayerName))
                        throw new ParasiteArgumentException($"{parentLayerName} is not a valid layer name");

                    if (string.IsNullOrEmpty(parentLayerName))
                        throw new ParasiteArgumentException($"{parentLayerName} cant be blank");
                }

                if (parentLayerName == null)
                    throw new ParasiteArgumentException($"Could not retrieve Parameter of type {ParameterType.Layer} ");

                if (parasiteObjects[i].GetParameter(ParameterType.ChildLayer.ToString(), out Parameter val))
                {
                    childLayerName = val.GetValue();

                    if (!Rhino.DocObjects.Layer.IsValidName(childLayerName))
                        throw new ParasiteArgumentException($"{childLayerName} is not a valid layer name");

                    if (string.IsNullOrEmpty(childLayerName))
                        throw new ParasiteArgumentException($"{childLayerName} cant be blank");
                }

                if (childLayerName == null)
                    throw new ParasiteArgumentException($"Could not retrieve Parameter of type {ParameterType.Layer} ");




                //Set layer
                if (!string.IsNullOrEmpty(parentLayerName) && Rhino.DocObjects.Layer.IsValidName(parentLayerName))
                {
                    //Get the current layer index
                    Rhino.DocObjects.Tables.LayerTable layerTable = DOC.Layers;
                    int parentLayerIndex = layerTable.Find(parentLayerName, true);

                    if (parentLayerIndex < 0) //This layer does not exist, we add it
                    {

                        Rhino.DocObjects.Layer onlayer = new Rhino.DocObjects.Layer
                        {

                            Name = parentLayerName,
                            Color = System.Drawing.Color.FromArgb(ran.Next(0, 255), ran.Next(0, 255), ran.Next(0, 255))
                        }; //Make a new layer

                        parentLayerIndex = layerTable.Add(onlayer); //Add the layer to the layer table

                        if (parentLayerIndex > -1) //We manged to add layer!
                        {
                            att.LayerIndex = parentLayerIndex;
                            att.LayerIndex = AddChildLayer(parentLayerName,parentLayerIndex, childLayerName);



                        }
                        else
                            throw new ParasiteArgumentException(string.Format("Unable to add {0} layer.", parentLayerName));
                    }

                    // Layer already exists
                    else
                    {
                        att.LayerIndex = parentLayerIndex;
                        att.LayerIndex = AddChildLayer(parentLayerName, parentLayerIndex, childLayerName);

                    }

                    
                }


                Rhino.DocObjects.Layer childLayer = DOC.Layers[att.LayerIndex];

                if (childLayer.Name == "Default") throw new ParasiteArgumentException("Child Layer was set to default!");


                if (parasiteObjects[i].Data is Parasite_BrepSolid solid)
                {

                     DOC.Objects.AddBrep(RhinoConversion.ToRhinoType(solid, 0.001),att);
                
                }

     
            }


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

using ParasiteIO.Core.Data.Parameter;
using ParasiteIO.Core.Exceptions;
using ParasiteIO.Core.Types;
using Rhino.DocObjects;
using Rhino.DocObjects.Tables;
using System;
using System.Collections.Generic;


/// <summary>
/// 
/// </summary>
namespace ParasiteIO.Core.Document.RhinoDocument
{
    /// <summary>
    /// 
    /// </summary>
    public class LayerFactory
    {
        #region FIELDS

        private static readonly Random ran = new Random();

        #endregion

        #region METHODS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentLayerName"></param>
        /// <param name="parentLayerIndex"></param>
        /// <param name="childLayerName"></param>
        /// <returns></returns>
        public static int AddChildLayer(string parentLayerName, int parentLayerIndex, string childLayerName, Rhino.RhinoDoc doc)
        {


            if (!Rhino.DocObjects.Layer.IsValidName(childLayerName))
                throw new ParasiteArgumentException($"{childLayerName} is not a valid layer name");

            if (string.IsNullOrEmpty(childLayerName))
                throw new ParasiteArgumentException($"{childLayerName} cant be blank");



            if (parentLayerIndex < 0)
                throw new ParasiteArgumentException($"The parent layer specified:{parentLayerName} does not exists in the current Rhino Document");

            Rhino.DocObjects.Layer parent_layer = doc.Layers[parentLayerIndex];


            int childLayerIndex = doc.Layers.Find(childLayerName, true);

            if (childLayerIndex < 0) //This layer does not exist, we add it
            {
                LayerTable layerTable = doc.Layers;

                int R = parent_layer.Color.R , G = parent_layer.Color.G  , B = parent_layer.Color.B;
         
                Layer childlayer = new Layer
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
        /// <param name="layerName"></param>
        private static void AssertLayerNameValidity(string layerName)
        {
            if (!Layer.IsValidName(layerName))
                throw new ParasiteArgumentException($"{layerName} is not a valid layer name");

            if (string.IsNullOrEmpty(layerName))
                throw new ParasiteArgumentException($"{layerName} cant be blank");
        }





        /// <summary>
        /// Creates layers in the current Rhino Document, by using the layer data contained within each 
        /// Parasite Object
        /// </summary>
        /// <param name="parasiteObjects"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<ObjectAttributes> CreateLayers(List<ParasiteObject> parasiteObjects, Rhino.RhinoDoc doc)
        {
            List<ObjectAttributes> objectAttributes = new List<ObjectAttributes>();

            for (int i = 0; i < parasiteObjects.Count; i++)
            {
                //Make new attribute to set name
                ObjectAttributes att = new ObjectAttributes();

                string parentLayerName = null;
                string childLayerName = null;

                if (parasiteObjects[i].GetParameter(ParameterType.Layer.ToString(), out Parameter value))
                {
                    parentLayerName = value.GetValue();
                    AssertLayerNameValidity(parentLayerName);
                }
               

                if (parasiteObjects[i].GetParameter(ParameterType.ChildLayer.ToString(), out Parameter val))
                {
                    childLayerName = val.GetValue();
                    AssertLayerNameValidity(childLayerName);
                }

                if (childLayerName == null)
                    throw new ParasiteArgumentException($"Could not retrieve Parameter of type {ParameterType.Layer} ");
                if (parentLayerName == null)
                    throw new ParasiteArgumentException($"Could not retrieve Parameter of type {ParameterType.Layer} ");


                //Set layer
                if (!string.IsNullOrEmpty(parentLayerName) && Rhino.DocObjects.Layer.IsValidName(parentLayerName))
                {
                    //Get the current layer index
                    Rhino.DocObjects.Tables.LayerTable layerTable = doc.Layers;
                    int parentLayerIndex = layerTable.Find(parentLayerName, true);

                    if (parentLayerIndex < 0) //This layer does not exist, we add it
                    {

                        Layer onlayer = new Layer
                        {
                            Name = parentLayerName,
                            Color = System.Drawing.Color.FromArgb(ran.Next(0, 255), ran.Next(0, 255), ran.Next(0, 255))
                        }; //Make a new layer

                        parentLayerIndex = layerTable.Add(onlayer); //Add the layer to the layer table

                        if (parentLayerIndex > -1) //We manged to add layer!
                        {
                            att.LayerIndex = parentLayerIndex;
                            att.LayerIndex = LayerFactory.AddChildLayer(parentLayerName, parentLayerIndex, childLayerName,doc);
                        }

                        else
                            throw new ParasiteArgumentException(string.Format("Unable to add {0} layer.", parentLayerName));
                    }

                    // Layer already exists
                    else
                    {
                        att.LayerIndex = parentLayerIndex;
                        att.LayerIndex = AddChildLayer(parentLayerName, parentLayerIndex, childLayerName,doc);

                    }


                }


                Layer childLayer = doc.Layers[att.LayerIndex];

                if (childLayer.Name == "Default") 
                    throw new ParasiteArgumentException("Child Layer was set to default!");


                objectAttributes.Add(att);
            }

            return objectAttributes;
        }

        #endregion


    }
}

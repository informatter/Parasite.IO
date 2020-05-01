using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using DynCategory = Revit.Elements.Category;
using DynamoSolid = Autodesk.DesignScript.Geometry.Solid;
using Autodesk.DesignScript.Runtime;

using ParasiteIO.Core.Types;
using ParasiteIO.Core.Data.Properties;
using ParasiteIO.Core.Exceptions;
using ParasiteIO.Dynamo.Core;

namespace ParasiteIO.Dynamo
{

    /// <summary>
    /// This class contains appropriate methods to select elements and their data
    /// from an active Revit document
    /// </summary>
    public class ElementSelection
    {
        #region FIELDS
        readonly static Document DOC = DocumentManager.Instance.CurrentDBDocument;
        readonly static Options OP = new Options();
        #endregion

        #region CONSTRCUTORS
        private ElementSelection() { }

        #endregion


        #region METHODS


        /// <summary>
        /// 
        /// </summary>
        /// <param name="geoElement"></param>
        /// <param name="outPutSolids"></param>
        private static void ProcessStairs(GeometryElement geoElement, List<DynamoSolid> outPutSolids)
        {

            for (int j = 0; j < geoElement.Count(); j++)
            {
                            
                GeometryInstance geoIns = geoElement.ElementAt(j) as GeometryInstance;

                //GetInstanceGeometry returns the geometry represented in the coordinate system of the project where the instance is placed.
                // https://thebuildingcoder.typepad.com/blog/2010/01/geometry-options.html
                GeometryElement instanceGeo = geoIns.GetInstanceGeometry();

                foreach (var item in instanceGeo)
                {
                    if (item is Solid)
                    {
                        Solid solid = item as Solid;

                        // Skip item if does not contain solid geo
                        if (solid == null) continue;

                        DynamoSolid solidGeo = GeometryExtraction.ToDynamoSolid(solid);

                        // Skip item if does not contain solid geo
                        if (solidGeo == null) continue;

                        outPutSolids.Add(solidGeo);

                    }
                }

            }

        }



        /// <summary>
        /// Extract solid geometries from a specific Revit Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        /// 
        // Remarks: This method should return Parasite Objects and dynamo solids for visualization
        // Parasite Objects enable each solid  to be identified if it is glass, window frame, mullion frame, wall.. ect         

        [MultiReturn(new[] { "Preview", "Parasite Objects" })]
        public static Dictionary<string, object> ExtractSolidsFromCategory(DynCategory category)
        {
            // Get specific elements from model by category
            ElementId targetCatId = new ElementId(category.Id);

            List<Element> elements = new FilteredElementCollector(DOC).OfCategoryId(targetCatId)
            .Cast<Element>().ToList();

            OP.ComputeReferences = true;
            OP.IncludeNonVisibleObjects = false;
            OP.DetailLevel = ViewDetailLevel.Fine;

            List<DynamoSolid> outPutSolids = new List<DynamoSolid>();
            List<ParasiteObject> parasiteObjects = new List<ParasiteObject>();

            for (int i = 0; i < elements.Count; i++)
            {
              
                if (elements[i].Category.Id == targetCatId)
                {
                    // This typically happens when the element is a FamilySymbol.
                    // FamilySymbols are loaded in document, they live in RFA files
                    // But they are not placed in the project. The families that are placed are
                    // Family Instances

                    if (elements[i] is FamilySymbol) continue;

                    else if (elements[i] is FamilyInstance)                   
                        GeometryExtraction.GeometryDataFromFamilyInstance(elements[i], outPutSolids,parasiteObjects, OP, DOC);
                       
                    
                    // This does not work for all stairs. It wont break but it simply wont process
                    // some conditions inside ProcessStairs() will never be met
                    else if (elements[i].Category.Name == "Stairs")
                    {

                        GeometryElement geoE = elements[i].get_Geometry(OP);

                        if (geoE == null) continue;
                        ProcessStairs(geoE, outPutSolids);
                    }


                    else if (elements[i].Category.Name == "Walls")                    
                        GeometryExtraction.GeometryDataFromWall(elements[i], outPutSolids, parasiteObjects, OP, DOC);
                          

                    else if (elements[i].Category.Name == "Floors")                    
                        GeometryExtraction.GeometryDataFromFloor(elements[i], outPutSolids, parasiteObjects, OP);
                    
                    else if (elements[i].Category.Name == "Roofs")                   
                        GeometryExtraction.GeometryDataFromRoof(elements[i], outPutSolids, parasiteObjects, OP);
                    

                    else
                        throw new ParasiteNotImplementedExceptions("Category not implemented yet");


                }
            }

         

            return new Dictionary<string, object>
            {
                 {"Preview",outPutSolids },
                 {"Parasite Objects", parasiteObjects}
            };
           
       
    }


      

        #endregion

    }
}

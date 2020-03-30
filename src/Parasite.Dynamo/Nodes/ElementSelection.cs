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

                        DynamoSolid solidGeo = GeometryUtility.ToDynamoSolid(solid);

                        // Skip item if does not contain solid geo
                        if (solidGeo == null) continue;

                        outPutSolids.Add(solidGeo);

                    }
                }

            }

        }

        
        /// <summary>
        /// This method selects  all family instances from a given category, and returns
        /// a collection of Parasite Objects. Each Parasite Object will contain Revit data of each
        /// of the geometry. For example a given frame of a window will identify itself as a frame
        /// </summary>
        /// <param name="category">A category to filter with</param>
        /// <returns>A collection of Parasite Objects </returns>
        [MultiReturn(new[] { "Parasite Objects", "Geometry"})]    
        public static Dictionary<string, object> ExtractSolidsFromFamilyCategory(DynCategory category)
        {
            /// *** NOTES
            // The big question 
            /// *** NOTES

            ElementId targetCatId = new ElementId(category.Id);
            List<Element> elements = new FilteredElementCollector(DOC).OfCategoryId(targetCatId)
            .Cast<Element>().ToList();

            // Just for previewing purposes in Dynamo
            List<DynamoSolid> outPutSolids = new List<DynamoSolid>();

            // List to store all Parasite Objects
            List<ParasiteObject> parasiteObjects = new List<ParasiteObject>(); 

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Category.Id == targetCatId)
                {

                    if (elements[i] is FamilyInstance)
                    {
                        GeometryElement geoElement = elements[i].get_Geometry(OP);
                        if (geoElement == null) continue;

                        for (int j = 0; j < geoElement.Count(); j++)
                        {

                            GeometryInstance geoInstance = geoElement.ElementAt(j) as GeometryInstance;
                            GeometryElement geometryElement = geoInstance.GetInstanceGeometry();

                            foreach (var item in geometryElement)
                            {
                                Solid solid = item as Solid;
                                DynamoSolid solidGeo = GeometryUtility.ToDynamoSolid(solid);

                                // Sometimes a geometry element can be a null solid, in this case
                                // skip the element and go to the next element in the iterated collection
                                if (solidGeo == null) continue;

                                outPutSolids.Add(solidGeo);
                                ElementId id = solid.GraphicsStyleId;
                                GraphicsStyle gStyle = DOC.GetElement(id) as GraphicsStyle;

                                // Assign any category for know
                                Property prop = new Property(ParasiteCategories.Roof, gStyle.GraphicsStyleCategory.Name);
                                ParasiteObject parasiteObj = new ParasiteObject (solidGeo, prop);
                                parasiteObjects.Add(parasiteObj);


                            }
                        }
                    }
                }
            }

            
            return new Dictionary<string, object>
            {
                { "Parasite Objects",parasiteObjects},
                { "Geometry",outPutSolids}
       
            };

        }




        /// <summary>
        /// Extract solid geometries from a specific Revit Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        /// 
        // Remarks: This method should return Parasite Objects and dynamo solids for visualization
        // Parasite Objects enable each solid  to be identified if it is glass, window frame, mullion frame, wall.. ect         

        public static List<DynamoSolid> ExtractSolidsFromCategory(DynCategory category)
        {
            // Get specific elements from model by category
            ElementId targetCatId = new ElementId(category.Id);

            List<Element> elements = new FilteredElementCollector(DOC).OfCategoryId(targetCatId)
            .Cast<Element>().ToList();

            OP.ComputeReferences = true;
            OP.IncludeNonVisibleObjects = false;
            OP.DetailLevel = ViewDetailLevel.Fine;

            List<DynamoSolid> outPutSolids = new List<DynamoSolid>();


            for (int i = 0; i < elements.Count; i++)
            {

                if (elements[i].Category.Id == targetCatId)
                {

                    

                    if (elements[i] is FamilyInstance)
                    {
                        // GeometryElement geoE = elements[i].get_Geometry(OP);
                        // if (geoE == null) continue;
                        // ProcessFamilyInstance(geoE, outPutSolids);

                        throw new ParasiteArgumentException("Family instances are not supported in this node! " +
                            "please use `Extract Solids FromFamily Category` node instead");
                    }

                    // This does not work for all stairs. It wont break but it simply wont process
                    // some conditions inside ProcessStairs() will never be met
                    else if (elements[i].Category.Name == "Stairs")
                    {                     
                        
                        GeometryElement geoE = elements[i].get_Geometry(OP);

                        if (geoE == null) continue;
                        ProcessStairs(geoE, outPutSolids);
                    }

       
                    else if(elements[i].Category.Name =="Walls")
                    {
                       
                        if (elements[i] is WallType)
                        {
                            // Cast Element to WallType
                            WallType wallType = elements[i] as WallType;

                            // Get Geometry Element
                            GeometryElement geoE = wallType.get_Geometry(OP);

                            // Skip if null
                            if (geoE == null) continue;

                            for (int j = 0; j < geoE.Count(); j++)
                            {
                                GeometryUtility.SolidFromGeometryObject(geoE.ElementAt(j), outPutSolids);

                            }


                        } // END WALLTYPE CLASS CONDITION
                        

                        else if (elements[i] is Wall)
                        {

                            Wall wall = elements[i] as Wall;

                            CurtainGrid cgrid = wall.CurtainGrid;

                            // This means this wall is a curtain grid
                            if (cgrid != null)
                            {
                              
                                ICollection<ElementId> mullionIds = cgrid.GetMullionIds();
                                ICollection<ElementId> panelIds = cgrid.GetPanelIds();
                                Element[] panels = panelIds.Select(a => DOC.GetElement(a)).ToArray();

                                for (int p = 0; p < panels.Length; p++)
                                {
                                    // Ignores Family instances in the Curtain wall. For example doors
                                    if (!(panels[p] is Panel panel)) continue;

                                    GeometryElement geometryElement = panel.get_Geometry(OP);

                                    foreach (GeometryObject item in geometryElement)
                                    {
                                        GeometryUtility.SolidFromGeometryObject(item, outPutSolids);                                    
                                    }

                                }


                                Element[] mullions = mullionIds.Select(a => DOC.GetElement(a)).ToArray();

                                for (int m = 0; m < mullions.Length; m++)
                                {
                                    Mullion mullion = mullions[m] as Mullion;

                                    GeometryElement geometryElement = mullion.get_Geometry(OP);

                                    foreach (var item in geometryElement)
                                    {
                                        GeometryUtility.SolidFromGeometryObject(item, outPutSolids);                                   
                                    }
                                }

                          

                            } // End of wall that is a curtain wall

                            // This means its a normal wall
                            else
                            {
                                GeometryElement geoE = elements[i].get_Geometry(OP);
                                if (geoE == null) continue;

                                for (int j = 0; j < geoE.Count(); j++)
                                {
                                    GeometryUtility.SolidFromGeometryObject(geoE.ElementAt(j), outPutSolids);
                                }
                            }
                        } // END WALL CLASS CONDITION

                        else
                            throw new ParasiteNotImplementedExceptions("Wall type not implemented yet");

                    }

                    else                   
                        throw new ParasiteNotImplementedExceptions("Category not implemented yet");
                    
                   
                }
            }

            return outPutSolids;
        }


      

        #endregion

    }
}

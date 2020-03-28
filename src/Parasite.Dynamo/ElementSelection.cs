using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Autodesk.Revit.DB;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using DynamoCat = Revit.Elements.Category;
using DynamoSolid = Autodesk.DesignScript.Geometry.Solid;
using ParasiteIO.Core.Types;
using ParasiteIO.Core.Data.Properties;
using Autodesk.DesignScript.Runtime;


namespace ParasiteIO.Dynamo
{

    /// <summary>
    /// 
    /// </summary>
    public class ElementSelection
    {
        readonly static Document DOC = DocumentManager.Instance.CurrentDBDocument;
        readonly static Options OP = new Options();
        private ElementSelection() { }



        /// <summary>
        /// process a Family instance and get its solid geometry
        /// </summary>
        /// <param name="geoElement"></param>
        /// <param name="geo"></param>
        private static void ProcessFamilyInstance(GeometryElement geoElement, List<DynamoSolid> geo)
        {
           
            for (int j = 0; j < geoElement.Count(); j++)
            {
                       
                GeometryInstance geoInstance = geoElement.ElementAt(j) as GeometryInstance;
                GeometryElement geometryElement = geoInstance.GetInstanceGeometry();

                foreach (var item in geometryElement)
                {
                    Solid solid = item as Solid;
                    if (solid.ToProtoType() == null) continue;
                    geo.Add(solid.ToProtoType());
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
        public static Dictionary<string, object> SelectFamilyFromCategory(DynamoCat category)
        {
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
                                DynamoSolid solidGeo = ToDynamoSolid(solid);

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
        /// Wrapper for ToProtoType() method so the debugger
        /// does not step in the method, but still executes it
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [DebuggerStepThroughAttribute]
        private static DynamoSolid ToDynamoSolid(Solid s)
        {
            return s.ToProtoType();
        }


        /// <summary>
        /// Select solid geometry from a specific Revit Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static List<DynamoSolid> SelectElementsFromCategory(DynamoCat category)
        {
            // Get specific elements from model by category

            
            ElementId targetCatId = new ElementId(category.Id);

            List<Element> elements = new FilteredElementCollector(DOC).OfCategoryId(targetCatId)
            .Cast<Element>().ToList();

            List<DynamoSolid> outPutSolids = new List<DynamoSolid>();


            for (int i = 0; i < elements.Count; i++)
            {
               
                if (elements[i].Category.Id == targetCatId)
                {
                    

                    if (elements[i] is FamilyInstance)
                    {
                        GeometryElement geoE = elements[i].get_Geometry(OP);
                        if (geoE == null) continue;
                        ProcessFamilyInstance(geoE, outPutSolids);
                    }

                    else
                    {
                        GeometryElement geoE = elements[i].get_Geometry(OP);
                        if (geoE == null) continue;

                        for (int j = 0; j < geoE.Count(); j++)
                        {
                            Solid s = geoE.ElementAt(j) as Solid;
                            if (s.ToProtoType() == null) continue;
                            outPutSolids.Add(s.ToProtoType());
                        }
                    }


                }
            }

            return outPutSolids;
        }








    }
}

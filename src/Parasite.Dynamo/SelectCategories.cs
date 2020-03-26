using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;
using DynamoElement = Revit.Elements.Element;
using DynamoCat = Revit.Elements.Category;
using DynamoSolid = Autodesk.DesignScript.Geometry.Solid;

using DB = Autodesk.Revit.DB;


namespace Parasite.Dynamo
{

    /// <summary>
    /// 
    /// </summary>
    public class SelectCategories
    {
        readonly static Document DOC = DocumentManager.Instance.CurrentDBDocument;
        readonly static Options OP = new Options();
        private SelectCategories() { }



        /// <summary>
        /// 
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

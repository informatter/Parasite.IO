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


namespace Parasite.Dynamo.Nodes
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
        /// <param name="category"></param>
        /// <returns></returns>
        
        public static List<DynamoSolid> SelectElementsFromCategory(DynamoCat category)
        {
            // Get specific elements from model by category

            ElementId targetCatId = new ElementId(category.Id);
         
            List<Element> elements = new FilteredElementCollector(DOC).OfCategoryId(targetCatId)   
            .Cast<Element>().ToList();

            List<DynamoSolid> geo = new List<DynamoSolid>();

            for (int i = 0; i < elements.Count; i++)
            {

                if (elements[i].Category.Id == targetCatId)
                {
                   GeometryElement geoE = elements[i].get_Geometry(OP);

                    if (geoE == null) continue;

                    for (int j = 0; j < geoE.Count(); j++)
                    {
                        Solid s = geoE.ElementAt(j) as Solid;

                        if (s.ToProtoType() == null) continue;

                        geo.Add(s.ToProtoType());
                    }

                    
                }
            }

            return geo;
        }
    }
}

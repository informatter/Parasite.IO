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
        /// ewdewiiwend
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



        /// <summary>
        /// THIS IS A TEMPORARY METHOD
        /// </summary>
        /// <param name="solids"></param>
        /// <returns></returns>
        public static List<Autodesk.DesignScript.Geometry.Curve> DisplayLoops(List<DynamoSolid> solids)
        {
            List<Autodesk.DesignScript.Geometry.Curve> curves = new List<Autodesk.DesignScript.Geometry.Curve>();

            for (int i = 0; i < solids.Count; i++)
            {
                Autodesk.DesignScript.Geometry.Face [] faces = solids[i].Faces;

                for (int j = 0; j < faces.Length; j++)
                {
                    Autodesk.DesignScript.Geometry.Loop [] loops = faces[j].Loops;

                    for (int k = 0; k < loops.Length; k++)
                    {
                       Autodesk.DesignScript.Geometry.CoEdge[] coEdge = loops[k].CoEdges;


                        for (int m = 0; m < coEdge.Length; m++)                                         
                            curves.Add(coEdge[m].Edge.CurveGeometry);
                        
                    }

  
                 
                }
            }


            return curves;
        }



        public static List<Solid> SelectElementsFromCategoryB(DynamoCat category)
        {
            // Get specific elements from model by category

            ElementId targetCatId = new ElementId(category.Id);

            List<Element> elements = new FilteredElementCollector(DOC).OfCategoryId(targetCatId)
            .Cast<Element>().ToList();

            List<Solid> geo = new List<Solid>();

            for (int i = 0; i < elements.Count; i++)
            {

                if (elements[i].Category.Id == targetCatId)
                {
                    GeometryElement geoE = elements[i].get_Geometry(OP);

                    if (geoE == null) continue;

                    for (int j = 0; j < geoE.Count(); j++)
                    {
                        Solid s = geoE.ElementAt(j) as Solid;


                        geo.Add(s);
                    }


                }
            }

            return geo;
        }





    }
}

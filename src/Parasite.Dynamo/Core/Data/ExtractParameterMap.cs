using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParasiteIO.Dynamo.Core.Data
{
    [IsVisibleInDynamoLibrary(false)]
    public class ExtractParameterMap
    {


        /// <summary>
        /// Retreive a Parameter Array from a Revit Element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Parameter[] RetreiveParameterMap(Element element)
        {
            ParameterMap parameterMap = element.ParametersMap;

            List<Parameter> parameters = new List<Parameter>();
         
            foreach (Parameter item in parameterMap)            
                parameters.Add(item);
          
            return parameters.ToArray();
        }


        /// <summary>
        /// Return the corresponding Storage Type value of a given Revit Parameter as a string representation
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Returns null if input parameter is also null </returns>
        public static string GetParameterValueAsString(Parameter p)
        {
            string output = "";
            if (p != null)
            {
                switch (p.StorageType)
                {

                    case StorageType.Double:
                        {
                            string data = p.AsDouble().ToString();
                            if (data != null) output = data;

                            else output = "none";
                            break;
                        }
                    case StorageType.Integer:
                        {
                            string data = p.AsInteger().ToString();
                            if (data != null) output = data;
                            else output = "none";

                            break;
                        }
                    case StorageType.String:
                        {
                            string data = p.AsString();
                            if (data != null) output = data;
                            else output = "none";
                            break;
                        }
                    case StorageType.ElementId:
                        {
                            string data = p.AsElementId().IntegerValue.ToString();
                            if (data != null) output = data;
                            else output = "none";
                            break;
                        }
                    case StorageType.None:
                        {
                            output = "none";
                            break;
                        }
                }
            }
            return output;
        }
    }
}

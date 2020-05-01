using Autodesk.Revit.DB;
using ParasiteIO.Core.Types;
using Revit.GeometryConversion;
using System.Diagnostics;
using DynamoSolid = Autodesk.DesignScript.Geometry.Solid;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using ParasiteIO.Core.Data.Properties;
using ParasiteIO.Core.Exceptions;
using System.Runtime.InteropServices;
using ParasiteIO.Core.Factory;

using P_Parameter = ParasiteIO.Core.Data.Parameter.Parameter;
using P_StorageType = ParasiteIO.Core.Data.Parameter.StorageType;
using P_ParameterType = ParasiteIO.Core.Data.Parameter.ParameterType;

namespace ParasiteIO.Dynamo.Core
{

    /// <summary>
    /// 
    /// </summary>
    internal class GeometryExtraction
    {

        

        /// <summary>
        /// This method selects  all family instances from a given category, and returns
        /// a collection of Parasite Objects. Each Parasite Object will contain Revit data of each
        /// of the geometry. For example a given frame of a window will identify itself as a frame
        /// </summary>
        /// <param name="element"></param>
        /// <param name="outPutSolids"></param>
        /// <returns></returns>
        public static void GeometryDataFromFamilyInstance(Element element, List<DynamoSolid> outPutSolids, 
            List<ParasiteObject> parasiteObjects, Options OP,Document doc)
        {

            if (element.Category.Name == "Windows")
            {

                GeometryElement geoElement = element.get_Geometry(OP);

                for (int i = 0; i < geoElement.Count(); i++)
                {

                    GeometryInstance geoInstance = geoElement.ElementAt(i) as GeometryInstance;
                    GeometryElement geometryElement = geoInstance.GetInstanceGeometry();

                    if (geometryElement != null)
                    {
                       
                        foreach (GeometryObject item in geometryElement)
                        {
                           
                            bool success = TryeGetGeometryDataFromGeometryObject(item, out Solid solid, out DynamoSolid dynS);
                            ParasiteObject parasiteObject = new ParasiteObject();

                            if (success)
                            {
                                 // GraphicsStyle will never be null for windows
                                if (doc.GetElement(solid.GraphicsStyleId) is GraphicsStyle gStyle)
                                { 

                                    P_Parameter paramChildLayer = new P_Parameter(P_ParameterType.ChildLayer, gStyle.GraphicsStyleCategory.Name, P_StorageType.String);
                                    P_Parameter paramLayer = new P_Parameter(P_ParameterType.Layer, element.Category.Name, P_StorageType.String);

                                    parasiteObject = DynamoParasiteFactory.CreateParasiteObject(dynS);
                                    DynamoParasiteFactory.AddParametersToParasiteObj(parasiteObject, new P_Parameter[] { paramChildLayer, paramLayer });
                                    parasiteObjects.Add(parasiteObject);
                                    outPutSolids.Add(dynS);

                                }

                                    else
                                        throw new ParasiteConversionExceptions(element.GetType(), parasiteObject.GetType());
                            }

                        }
                    }
                }
            }

            else if(element.Category.Name== "Doors")
            {
                             
                GeometryElement geoElement = element.get_Geometry(OP);
                P_Parameter paramChildLayer = new P_Parameter(P_ParameterType.ChildLayer, element.Name, P_StorageType.String);
                P_Parameter paramLayer = new P_Parameter(P_ParameterType.Layer, element.Category.Name, P_StorageType.String);

                for (int i = 0; i < geoElement.Count(); i++)
                {

                    GeometryInstance geoInstance = geoElement.ElementAt(i) as GeometryInstance;
                    GeometryElement geometryElement = geoInstance.GetInstanceGeometry();

                    if (geometryElement != null)
                    {                   
                        DynamoParasiteFactory.CreateParasiteObjectFromGeometryElement(geometryElement,
                            new P_Parameter[] { paramChildLayer, paramLayer }, outPutSolids, parasiteObjects);
                    }
                }
            }

            else
                throw new ParasiteNotImplementedExceptions("Family Instance Category not implemented yet");

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="outPutSolids"></param>
        /// <param name="parasiteObjects"></param>
        /// <param name="op"></param>
        /// <param name="doc"></param>
        public static void GeometryDataFromFloor(Element element, List<DynamoSolid> outPutSolids, 
            List<ParasiteObject> parasiteObjects, Options op)
        {
            if (element is Floor)
            {
                Floor floor = element as Floor;
                GeometryElement geometryElement = floor.get_Geometry(op);

                if (geometryElement != null)
                {
                    P_Parameter paramChildLayer = new P_Parameter(P_ParameterType.ChildLayer, floor.FloorType.Name, P_StorageType.String);
                    P_Parameter paramLayer = new P_Parameter(P_ParameterType.Layer, element.Category.Name, P_StorageType.String);

                    DynamoParasiteFactory.CreateParasiteObjectFromGeometryElement(geometryElement,
                        new P_Parameter[] { paramChildLayer, paramLayer }, outPutSolids, parasiteObjects);
                }
                    

                
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="outPutSolids"></param>
        /// <param name="parasiteObjects"></param>
        /// <param name="OP"></param>
        public static void GeometryDataFromRoof(Element element, List<DynamoSolid> outPutSolids,
            List<ParasiteObject> parasiteObjects, Options OP)
        {
            if (element is RoofBase)
            {
                RoofBase roofBase = element as RoofBase;
                GeometryElement geometryElement = roofBase.get_Geometry(OP);

                // Skip if null
                if (geometryElement != null)
                {
                    P_Parameter paramChildLayer = new P_Parameter(P_ParameterType.ChildLayer, roofBase.RoofType.Name, P_StorageType.String);
                    P_Parameter paramLayer = new P_Parameter(P_ParameterType.Layer, element.Category.Name, P_StorageType.String);

                    DynamoParasiteFactory.CreateParasiteObjectFromGeometryElement(geometryElement,
                        new P_Parameter[] { paramChildLayer, paramLayer }, outPutSolids, parasiteObjects);


                }
                  
            }
        }



                /// <summary>
                /// Extracts a Dynamo Solid from a Revit Wall object and creates a ParasiteObject 
                /// from the extracted geometry
                /// </summary>
                /// <param name="element"></param>
                /// <param name="outPutSolids"></param>
                /// <param name="parasiteObjects"></param>
                /// <param name="OP"></param>
                /// <param name="doc"></param>
                public static void GeometryDataFromWall(Element element, List<DynamoSolid> outPutSolids, 
            List<ParasiteObject> parasiteObjects, Options OP, Document doc)
        {

            if (element is WallType)
            {
                // Cast Element to WallType
                WallType wallType = element as WallType;

                // Get Geometry Element
                GeometryElement geometryElement = wallType.get_Geometry(OP);

                // Skip if null
                if (geometryElement != null)
                {
                    P_Parameter paramChildLayer = new P_Parameter(P_ParameterType.ChildLayer, wallType.Name, P_StorageType.String);
                    P_Parameter paramLayer = new P_Parameter(P_ParameterType.Layer, element.Category.Name, P_StorageType.String);

                    DynamoParasiteFactory.CreateParasiteObjectFromGeometryElement(geometryElement,
                        new P_Parameter[] { paramChildLayer, paramLayer }, outPutSolids, parasiteObjects);

                }
              
          

            } // END WALLTYPE CLASS CONDITION


            else if (element is Wall)
            {

                Wall wall = element as Wall;
                CurtainGrid cgrid = wall.CurtainGrid;

                //  THIS MEANS WALL IS A GLAZED CURTAIN WALL
                if (cgrid != null)
                {

                 
                    ICollection<ElementId> panelIds = cgrid.GetPanelIds();
                    Element[] panels = panelIds.Select(a => doc.GetElement(a)).ToArray();

                    for (int p = 0; p < panels.Length; p++)
                    {
                        // Ignores Family instances in the Curtain wall. For example doors
                        if (!(panels[p] is Panel panel)) continue;

                        GeometryElement geometryElement = panel.get_Geometry(OP);

                        if(geometryElement!=null)
                        {
                            P_Parameter paramChildLayer = new P_Parameter(P_ParameterType.ChildLayer, panel.Category.Name, P_StorageType.String);
                            P_Parameter paramLayer = new P_Parameter(P_ParameterType.Layer, element.Category.Name, P_StorageType.String);

                            DynamoParasiteFactory.CreateParasiteObjectFromGeometryElement(geometryElement,
                                new P_Parameter[] { paramChildLayer, paramLayer }, outPutSolids, parasiteObjects);
                        }
                    }

                    ICollection<ElementId> mullionIds = cgrid.GetMullionIds();
                    Element[] mullions = mullionIds.Select(a => doc.GetElement(a)).ToArray();

                    for (int m = 0; m < mullions.Length; m++)
                    {
                        Mullion mullion = mullions[m] as Mullion;

                       //if (!(mullions[m] is Mullion mullion)) continue;

                        GeometryElement geometryElement = mullion.get_Geometry(OP);

                        if (geometryElement != null)
                        {
                            P_Parameter paramChildLayer = new P_Parameter(P_ParameterType.ChildLayer, mullion.Category.Name, P_StorageType.String);
                            P_Parameter paramLayer = new P_Parameter(P_ParameterType.Layer, element.Category.Name, P_StorageType.String);

                            DynamoParasiteFactory.CreateParasiteObjectFromGeometryElement(geometryElement,
                                new P_Parameter[] { paramChildLayer, paramLayer }, outPutSolids, parasiteObjects);
                        }
                        
                    }



                } // End of wall that is a curtain wall

                // THIS MEANS ITS A NORMAL WALL
                else
                {
                 
                    GeometryElement geometryElement = element.get_Geometry(OP);
                    if (geometryElement != null)
                    {
                        P_Parameter paramChildLayer = new P_Parameter(P_ParameterType.ChildLayer, wall.WallType.Name, P_StorageType.String);
                        P_Parameter paramLayer = new P_Parameter(P_ParameterType.Layer, element.Category.Name, P_StorageType.String);

                        DynamoParasiteFactory.CreateParasiteObjectFromGeometryElement(geometryElement,
                               new P_Parameter[] { paramChildLayer, paramLayer }, outPutSolids, parasiteObjects);

                    }
                 
                    
                }
            } // END WALL CLASS CONDITION

            else
                throw new ParasiteNotImplementedExceptions("Wall type not implemented yet");

        }


        /// <summary>
        /// Wrapper for ToProtoType() method so the debugger
        /// does not step in the method, but still executes it
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [DebuggerStepThroughAttribute]
        public static DynamoSolid ToDynamoSolid(Solid s) => s.ToProtoType();




        /// <summary>
        /// 
        /// </summary>
        /// <param name="geoObject"></param>
        /// <param name="_outdynSolid"></param>
        /// <returns></returns>
        public static bool TryGetGeometryFromGeometryObject(GeometryObject geoObject, out DynamoSolid _outdynSolid)
        {
            DynamoSolid outdynSolid = null;
            bool success;

            // Check if its is a Geometry Instance
            if (geoObject is GeometryInstance)
            {
                GeometryInstance geoInstance = geoObject as GeometryInstance;

                foreach (Solid solid in geoInstance.GetInstanceGeometry())
                {
                    // Skip to next element in iteration if any of the following conditions are met                                   
                    if (null == solid || 0 == solid.Faces.Size || 0 == solid.Edges.Size)
                        continue;

                    DynamoSolid dynSolidGeo = ToDynamoSolid(solid);

                    // Sometimes a geometry element can be a null solid
                    if (dynSolidGeo != null)
                        outdynSolid = dynSolidGeo;

                }

            }

            // Check if it is Solid
            else if (geoObject is Solid)
            {
                Solid solid = geoObject as Solid;
                DynamoSolid dynSolidGeo = ToDynamoSolid(solid);

                // Sometimes a geometry element can be a null solid
                if (dynSolidGeo != null)
                    outdynSolid = dynSolidGeo;
            }

            if (outdynSolid == null) success = false;
            else
                success = true;

            _outdynSolid = outdynSolid;

            return success;
        }




        /// <summary>
        /// Extract a 3D solid from a Revit GeometryObject
        /// </summary>
        /// <param name="geoObject"></param>
        /// <param name="_outSolid"></param>
        /// <returns></returns>
        public static bool TryeGetGeometryDataFromGeometryObject(GeometryObject geoObject, out Solid _outSolid, out DynamoSolid _outDynSolid)
        {
            DynamoSolid outdynSolid = null;
            Solid outSolid = null;
            bool success;
            // Check if its is a Geometry Instance
            if (geoObject is GeometryInstance)
            {
                GeometryInstance geoInstance = geoObject as GeometryInstance;

                foreach (Solid solid in geoInstance.GetInstanceGeometry())
                {
                    // Skip to next element in iteration if any of the following conditions are met                                   
                    if (null == solid || 0 == solid.Faces.Size || 0 == solid.Edges.Size)
                        continue;

                    DynamoSolid dynSolidGeo = ToDynamoSolid(solid);

                    // Sometimes a geometry element can be a null solid
                    if (dynSolidGeo != null)
                    {
                        outdynSolid = dynSolidGeo;
                        outSolid = solid;
                    }

                }

            }

            // Check if it is Solid
            else if (geoObject is Solid)
            {
                Solid solid = geoObject as Solid;
                DynamoSolid dynSolidGeo = ToDynamoSolid(solid);

                // Sometimes a geometry element can be a null solid
                if (dynSolidGeo != null)
                {
                    outdynSolid = dynSolidGeo;
                    outSolid = solid;
                }

            }



            if (outdynSolid == null && outSolid == null) success = false;
            else
                success = true;

            _outDynSolid = outdynSolid;
            _outSolid = outSolid;

            return success;

        }




    }
}


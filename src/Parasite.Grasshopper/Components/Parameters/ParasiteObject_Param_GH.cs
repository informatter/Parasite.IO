using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Parasite.Grasshopper.Components.Types;
using ParasiteIO.Core.Types;
using ParasiteIO.Core.Types.Geometry;
using Rhino.Geometry;
using ParasiteIO.Core.Types.Wrappers.Grasshopper;

using ParasiteObject_GH = ParasiteIO.Core.Types.Wrappers.Grasshopper.ParasiteObject_GH;

namespace Parasite.Grasshopper.Components.Parameters
{
    public class ParasiteObject_Param_GH : GH_Param<ParasiteObject_GH> /*GH_PersistentGeometryParam<ParasiteObject_GH>*/, IGH_PreviewObject
    {
        List<object> m_geometry = new List<object>();
        readonly GH_Structure<ParasiteObject_GH> stuff;

        /// <summary>
        /// Initializes a new instance of the ParasiteObject_GH class.
        /// </summary>
        public ParasiteObject_Param_GH()
           : base(new GH_InstanceDescription("Parasite Object", "ParaObj", "", "Parasite.IO", "Parameters"))
        {
            //Figure out why it is volatile data
            stuff = this.VolatileData as GH_Structure<ParasiteObject_GH>;

            // Volatile data property is empty inside constructor
            //GH_Structure<ParasiteObject_GH> stuff = this.VolatileData as GH_Structure<ParasiteObject_GH>;

            //for (int i = 0; i < stuff.Branches.Count; i++)
            //{
            //    for (int j = 0; j < stuff.Branches[i].Count; j++)
            //    {
            //        ParasiteObject parasiteObject = stuff.Branches[i][j].Value;
            //        if (parasiteObject.Data is Parasite_BrepSolid)
            //        {
            //            Parasite_BrepSolid geo = parasiteObject.Data as Parasite_BrepSolid;
            //            Brep brep = ParasiteIO.Conversion.Rhinoceros.RhinoConversion.ToRhinoType(geo, 0.001);
            //            m_geometry.Add(brep);
            //        }
            //    }

            //}
        }

       

        #region PROPERTIES

        public BoundingBox ClippingBox { get => Preview_ComputeClippingBox(); }


        bool _hidden = false;
        public bool Hidden { get => _hidden; set => _hidden = value; }


        public bool IsPreviewCapable { get => true; }


        /// <inheritdoc />
        public override GH_Exposure Exposure { get => GH_Exposure.primary; }


        /// <summary>
        /// Provides an Icon for the component.
        ///You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon { get => null; }


        #endregion




        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid { get => new Guid("7b426fbe-f891-4fb6-a8ae-9dddbd17716c"); }


        ///// <inheritdoc />

        //protected override GH_GetterResult Prompt_Singular(ref ParasiteObject_GH value)  => GH_GetterResult.cancel; 

        ///// <inheritdoc />
        //protected override GH_GetterResult Prompt_Plural(ref List<ParasiteObject_GH> values) => GH_GetterResult.cancel; 


        #region PREVIEW METHODS
     



        public void DrawViewportWires(IGH_PreviewArgs args)
        {

        }

       
        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            

          
            if (args.Document.PreviewMode == GH_PreviewMode.Shaded && args.Display.SupportsShading)
            {
                Parallel.For(0, stuff.Branches.Count, i =>
                 {
                     // for (int i = 0; i < stuff.Branches.Count; i++)
                     // {
                     for (int j = 0; j < stuff.Branches[i].Count; j++)
                     {
                         ParasiteObject parasiteObject = stuff.Branches[i][j].Value;
                         if (parasiteObject.Data is Parasite_BrepSolid)
                         {
                             Parasite_BrepSolid geo = parasiteObject.Data as Parasite_BrepSolid;
                             Brep brep = ParasiteIO.Conversion.Rhinoceros.RhinoConversion.ToRhinoType(geo, 0.001);
                             args.Display.DrawBrepShaded(brep, args.ShadeMaterial);
                         }
                     }

                     // }
                 });

                //for (int i = 0; i < m_geometry.Count; i++)
                //{
                //    if (m_geometry[i] is Brep)
                //    {
                //        Brep brep = m_geometry[i] as Brep;
                //        args.Display.DrawBrepShaded(brep, args.ShadeMaterial);
                //    }
                //}

            }
        }

        #endregion

    }
}

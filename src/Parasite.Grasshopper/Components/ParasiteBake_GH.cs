using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using ParasiteIO.Core.Types;
using ParasiteIO.Core.Document.RhinoDocument;
using Rhino.DocObjects;

namespace Parasite.Grasshopper.Components
{
    public class ParasiteBake_GH : GH_Component
    {
        private static readonly Rhino.RhinoDoc DOC = Rhino.RhinoDoc.ActiveDoc;
        /// <summary>
        /// Initializes a new instance of the ParasiteBake_GH class.
        /// </summary>
        public ParasiteBake_GH()
          : base("Bake", "Bake",
              "Description",
              "Parasite.IO", "Bake")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "D", "", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Bake", "B","",GH_ParamAccess.item);
            
        }

        

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            
            
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<ParasiteObject> _data = new List<ParasiteObject>();
            bool _bake = false;
            if (!DA.GetDataList(0, _data)) return;
            if (!DA.GetData(1, ref _bake)) return;

            if (_bake)
            {
                //BakeToLayer.BakeTo(_data);
                AddGeometryToDocument.AddToDocument(_data, LayerFactory.CreateLayers(_data, DOC), DOC);
            }

           



        }

      

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f5c1c100-6710-4d37-89dc-bf4681feba12"); }
        }
    }
}
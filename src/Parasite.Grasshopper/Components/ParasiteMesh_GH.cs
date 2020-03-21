using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

using Parasite;

using Parasite.Conversion.Parasite;
using Parasite.Core.Types.Geometry;

namespace Parasite.Grasshopper.Components
{
    public class ParasiteMesh_GH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ParasiteMesh_GH class.
        /// </summary>
        public ParasiteMesh_GH()
          : base("ParasiteMesh", "PMesh",
              "Description",
              "Parasite.IO", "Types")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "A Rhino Mesh", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Mesh", "M", "A Parasite Mesh", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Mesh> meshes = new List<Mesh>();

            if (!DA.GetDataList(0, meshes)) return;

            DA.SetDataList(0, meshes.Select(a => ParasiteConversion.ToParasiteType(a)).ToList());

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
            get { return new Guid("c22d3bd9-c087-46aa-91af-3de1bc4c7ceb"); }
        }
    }
}
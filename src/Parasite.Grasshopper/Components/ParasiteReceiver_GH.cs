using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using Parasite.Core.Sync;

namespace Parasite.Grasshopper.Components
{

    /// <summary>
    /// 
    /// </summary>
    public class ParasiteReceiver_GH : GH_Component, IGH_VariableParameterComponent
    {
        Parasite.Core.Data.ReceiveDataFromParasite.ToGrasshopper toGrasshopper = new Core.Data.ReceiveDataFromParasite.ToGrasshopper();

        double tolerance;

        FolderListener folderListener = new FolderListener();
        /// <summary>
        /// Initializes a new instance of the ParasiteReceiver_GH class.
        /// </summary>
        public ParasiteReceiver_GH()
          : base("Receive", "Receive",
              "Receive data ",
              "Parasite.IO", "Receive Data")
        {
           tolerance = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Id", "Id", "Unique Id for the data", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "Data", "Some data coming out of here..", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string _id = "";
            DA.GetData(0, ref _id);
        
           List<List<object>>  data = toGrasshopper.ReceiveData(_id, tolerance);

            if (data.Count < Params.Output.Count)            
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "You have more incoming data than available outputs");

           
            for (int i = 0; i < Params.Output.Count; i++)
                DA.SetDataList(i, data[i]);
            
            if(folderListener.CanExpire)
            {
                this.ExpireSolution(true);
            }

        }



        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Output) return true;
            else return false;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Output && Params.Output.Count > 1) return true;
            else return false;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            Param_GenericObject param = new Param_GenericObject
            {
                Name = "Data" + " " + index.ToString(),
                NickName = "Data" + " " + index.ToString(),
                Description = "A data stream",
                Access = GH_ParamAccess.list

            };

            return param;
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public void VariableParameterMaintenance()
        {
            //throw new NotImplementedException();
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
            get { return new Guid("4052c98c-db4f-425f-93bb-8231c019cd9e"); }
        }
    }
}
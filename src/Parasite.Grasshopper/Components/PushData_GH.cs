using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Parasite.Core.Data;
using Parasite.Core.Types;
using Parasite.Core.Sync;
using Parasite.Core.Data.CollectData;

using Grasshopper.Kernel.Types;
using Parasite.Core.Types.Geometry;
using Rhino.Geometry.Collections;
using Parasite.Core.Data.CollectDataFromApplication;
using Parasite.Core.Collections;


namespace Parasite.Grasshopper
{
    public class PushData_GH : GH_Component, IGH_VariableParameterComponent
    {
        FromGrasshopper fromGrasshopper = new FromGrasshopper();
       

        /// <summary>
        /// Initializes a new instance of the Send class.
        /// </summary>
        public PushData_GH()
          : base("Send", "Send",
              "Send data ",
              "Parasite.IO", "Push")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Send", "S", "Send data", GH_ParamAccess.item);
            pManager.AddTextParameter("Id", "Id", "Unique Id for the data", GH_ParamAccess.item);
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

            bool _send = false;
            string _id = "";

            DA.GetData(0, ref _send);
            DA.GetData(1, ref _id);

            int threshold = 0;
            int inputCount = 0;

            for (int i = 0; i < Params.Input.Count; i++)
            {
                if (Params.Input[i].Name != "Data") threshold++;
                if (Params.Input[i].Name == "Data") inputCount++;
            }

            List<int> indexes = new List<int>();
         
            List<DataContainerFactory> _dataContainers = new List<DataContainerFactory>();

            // Iterate through params and find ZUI parameters (if there are any)
            for (int i = 0; i < Params.Input.Count; i++)
            {
                if (i > threshold - 1)
                {
                    // Set temp data
                    List<object> s = new List<object>();
                    _dataContainers.Add(new DataContainerFactory(s));
                    indexes.Add(i);
                }
            }


            // Retrieve data from ZUI parameters ( if there are any)
            // and assign it to _dataContainers
            for (int i = 0; i < _dataContainers.Count; i++)
            {
                DA.GetDataList(indexes[i], _dataContainers[i].data);
            }


            DataContainer dc = fromGrasshopper.CollectDataFromApplication(_dataContainers);

            PushData pd = new PushData();

            pd.PushDataLocal(dc, _id);
            
            Message = "Pushed @" + Environment.NewLine + string.Format("{0:HH:mm:ss tt}", DateTime.Now);
          
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Input) return true;
            else return false;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Input && Params.Input.Count > 2) return true;
            else return false;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {

            Param_GenericObject param = new Param_GenericObject
            {
                Name = "Data",
                NickName = "Data",
                Description = "sw",
                Optional = true,
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
            get { return new Guid("fb1988da-b3f7-48a0-a2d4-c3f7ca2d7791"); }
        }
    }
}
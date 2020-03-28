using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using ParasiteIO.Core.Sync;
using ParasiteIO.Core.Data.CollectDataFromApplication;
using ParasiteIO.Core.Collections;


namespace Parasite.Grasshopper
{
    public class ParasiteSender_GH : GH_Component, IGH_VariableParameterComponent
    {
        CollectDataFromGH fromGrasshopper = new CollectDataFromGH();


        /// <summary>
        /// Initializes a new instance of the Send class.
        /// </summary>
        public ParasiteSender_GH()
          : base("Send", "Send",
              "Send data ",
              "Parasite.IO", "Send Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
           // pManager.AddBooleanParameter("Send", "S", "Send data", GH_ParamAccess.item);
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

         //   bool _send = false;
            string _id = "";
            int threshold = 0;
            int ZUIInputCount = 0;

          //  DA.GetData(0, ref _send);
            DA.GetData(0, ref _id);

            for (int i = 0; i < Params.Input.Count; i++)
            {
                if (Params.Input[i].Name != "Data") threshold++;
                if (Params.Input[i].Optional == true) ZUIInputCount++;
            }


            List<int> indexes = new List<int>();
            List<List<object>> dataContainerTemp = new List<List<object>>();

    

            // Iterate through params and find ZUI parameters (if there are any)
            for (int i = 0; i < Params.Input.Count; i++)
            {
               
               if( Params.Input[i].Optional == true)
                {
                    // Set temp data
                    dataContainerTemp.Add(new List<object>());
                    indexes.Add(i);
                }

            }


            // Retrieve data from ZUI parameters ( if there are any)
            // and assign it to _dataContainers
            for (int i = 0; i < dataContainerTemp.Count; i++)
                DA.GetDataList(indexes[i], dataContainerTemp[i]); 
               

            DataContainer dc = fromGrasshopper.CollectData(dataContainerTemp);

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
                Description = "Data to send far far away to another galaxy. In the name of Carl Sagan",
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
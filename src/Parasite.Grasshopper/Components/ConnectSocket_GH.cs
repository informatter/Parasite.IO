using System;
using System.Collections.Generic;

using Grasshopper.Kernel;

using GH = Grasshopper;
using Rhino.Geometry;


using Quobject.SocketIoClientDotNet.Client;
using System.Windows.Forms;

namespace Parasite.Grasshopper.Components
{
    public class ConnectSocket_GH : GH_Component
    {
        private int count = 0;
        public Quobject.SocketIoClientDotNet.Client.Socket socket;
        private string status = "";

        /// <summary>
        /// Initializes a new instance of the ConnectSocket_GH class.
        /// </summary>
        public ConnectSocket_GH()
          : base("CreateSocket", "CS",
              "Description",
              "Parasite.IO", "Server")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Address", "Address", "IP Address to send to", GH_ParamAccess.item, "http://127.0.0.1:8080");
            pManager.AddBooleanParameter("Connect", "Connect", "Send data", GH_ParamAccess.item, false);
         
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Status", "Status", "Socket status", GH_ParamAccess.item);
            pManager.AddGenericParameter("Socket", "Socket", "Socket data", GH_ParamAccess.item);
            pManager.AddTextParameter("count", "count", "count", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string address = "";
            bool send = false;


            if (!DA.GetData(0, ref address)) return;
            if (!DA.GetData(1, ref send)) return;



            if (send)
            {
                if (count == 0)
                {
                    try
                    {

                        socket = Quobject.SocketIoClientDotNet.Client.IO.Socket(address);


                        socket.On(Socket.EVENT_CONNECT, () =>
                        {

                            status = "Connected";

                            socket.Emit("connected", "Grasshopper");

                            GH.Instances.DocumentEditor.Invoke((MethodInvoker)delegate
                            {
                                this.ExpireSolution(true);
                            });

                        });

                        socket.On(Socket.EVENT_ERROR, () =>
                        {
                            status = "Error";
                        });

                        socket.On(Socket.EVENT_DISCONNECT, () =>
                        {
                            status = "Disconnected";
                        });

                        socket.On(Socket.EVENT_RECONNECT, () =>
                        {
                            status = "Reconnected";
                        });



                        count++;
                    }
                    catch (System.UriFormatException e)
                    {
                        status = e.ToString();
                    }
                }


            }
            else
            {
                if (socket != null)
                {
                    socket.Emit("disconnect", "Grasshopper");
                    socket.Disconnect();
                }
                count = 0;
            }

            DA.SetData(0, status);
            DA.SetData(1, socket);
            DA.SetData(2, count);

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
            get { return new Guid("77333c35-64f8-4a50-a3da-bfe131176225"); }
        }
    }
}
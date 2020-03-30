using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RevitServices.Transactions;
using Revit.GeometryConversion;
using Revit.Elements;
using Autodesk.Revit.Creation;
using RevitServices.Persistence;
using System.Windows.Forms;

namespace Parasite.Dynamo
{
    public class SendToRevit
    {
        internal readonly static Autodesk.Revit.DB.Document DOC = DocumentManager.Instance.CurrentDBDocument;
        private SendToRevit() { }

        public static List<Autodesk.Revit.DB.GeometryObject> SendToCurrentRevitDocument(List<object> geo)
        {
            List<Autodesk.Revit.DB.GeometryObject> outPut = new List<Autodesk.Revit.DB.GeometryObject>();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Start();

            for (int i = 0; i < geo.Count; i++)
            {
                if(geo[i] is Autodesk.DesignScript.Geometry.Surface )
                {
                    Autodesk.DesignScript.Geometry.Surface srf = geo[i] as Autodesk.DesignScript.Geometry.Surface;
                    IList<Autodesk.Revit.DB.GeometryObject> dc = srf.ToRevitType();

               

                    // "Start" the transaction
                    TransactionManager.Instance.EnsureInTransaction(DOC);
              
                    for (int j = 0; j < dc.Count; j++)
                    {
                        outPut.Add(dc[j]);
                    }

                    // "End" the transaction
                    TransactionManager.Instance.TransactionTaskDone();
                }
            }

            sw.Stop();

            string data = string.Format("Time taken to load {0} elements: {1} seconds ", outPut.Count.ToString(), (sw.ElapsedMilliseconds * 0.001).ToString());



            MessageBox.Show(data, "Parasite.IO Data", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return outPut;
        }
    }
}

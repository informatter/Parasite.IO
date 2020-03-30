
using System;
using System.Collections.Generic;
using System.Windows;
using System.Threading.Tasks;

using ParasiteIO.Core.Sync;
using ParasiteIO.Dynamo.Wrappers.Geometry;
using ParasiteIO.Core.Collections;
using ParasiteIO.Core.Types.Geometry;
using ParasiteIO.Conversion.Dynamo;
using ParasiteIO.Core.Exceptions;

using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using System.Windows.Forms;
//using Dynamo.Graph.Nodes;
//using ProtoCore.AST.AssociativeAST;
//using Dynamo.Wpf;




namespace ParasiteIO.Dynamo
{

    /// <summary>
    /// 
    /// </summary>
    /// 

    //[NodeName("Parasite Receiver")]
    //[NodeDescription("Receives data from an external application")]
    //[NodeCategory("Parasite.ReceiveData")]

    //[OutPortNames("Report")]
    //[OutPortDescriptions("OutToolTip0")]
    //[OutPortTypes("Report")]
    public class ParasiteReceiver //: VariableInputNode//: IReceiveData
    {
        private ParasiteReceiver()
        {


        }



        /// <summary>
        /// Receives Parasite Data from a given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<object> ReceiveData(string id)
        {
            List<object> outPut = new List<object>();

            RequestData rd = new RequestData();

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            string data;

            sw.Start();

            DataContainer dataContainer = rd.RequestDataLocal(id);

            Parallel.For(0, dataContainer.Data.Length, i =>
             {

             // for (int i = 0; i < dataContainer.Data.Length; i++)
             // {
             for (int j = 0; j < dataContainer.Data[i].Length; j++)
             {
                 if (dataContainer.Data[i][j].Node is Parasite_Mesh mesh)
                 {
                     if (mesh.VertexColors == null)
                         outPut.Add(DynamoConversion.ToDynamoType(mesh));
                     else
                     {
                         Mesh m = DynamoConversion.ToDynamoType(mesh);
                         MeshWrapper wrapper = new MeshWrapper(m.VertexNormals, m.VertexPositions, mesh.VertexColors);
                         outPut.Add(wrapper);
                     }
                 }

                 else if (dataContainer.Data[i][j].Node is Parasite_NurbsCurve nurbsCurve)
                     outPut.Add(DynamoConversion.ToDynamoType(nurbsCurve));

                 else if (dataContainer.Data[i][j].Node is Parasite_BrepSurface brepSrf)
                     outPut.Add(DynamoConversion.ToDynamoType(brepSrf));
                 else
                     throw new ParasiteNotImplementedExceptions("Type conversion not implemented yet!");

             }
             //}

            });

            sw.Stop();



            data = string.Format("Time taken to load {0} elements: {1} seconds ", outPut.Count.ToString(), (sw.ElapsedMilliseconds * 0.001).ToString());



            MessageBox.Show(data, "Parasite.IO Data", MessageBoxButtons.OK, MessageBoxIcon.Information);


            return outPut;
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="index"></param>
        ///// <returns></returns>
        //protected override string GetInputName(int index)
        //{
        //    return "Data" + index;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="index"></param>
        ///// <returns></returns>
        //protected override string GetInputTooltip(int index)
        //{
        //    return "Foo" + index;
        //}

        //    public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        //    {
        //        if (IsPartiallyApplied)
        //        {
        //            return new[]
        //            {
        //                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode())
        //            };
        //        }

        //        AssociativeNode listNode = AstFactory.BuildExprList(inputAstNodes);

        //        var functionCall =
        //            AstFactory.BuildFunctionCall(
        //                new Func<List<object>, D3jsLib.Report>(MandrillTypes.Utilities.CreateGridsterReport),
        //                new List<AssociativeNode> { listNode });

        //        return new[]
        //        {
        //            AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall),
        //        };
        //    }
        //}


    }



}
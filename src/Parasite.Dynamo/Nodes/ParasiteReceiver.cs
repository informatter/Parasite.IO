using System;
using System.Collections.Generic;

using Parasite.Core.Sync;
using Parasite.Dynamo.Wrappers.Geometry;
using Parasite.Core.Collections;
using Parasite.Core.Types.Geometry;
using Parasite.Conversion.Dynamo;
using Parasite.Core.Exceptions;

using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using Dynamo.Wpf;



namespace Parasite.Dynamo
{

    /// <summary>
    /// 
    /// </summary>
    /// 

    [NodeName("Parasite Receiver")]
    [NodeDescription("Receives data from an external application")]
    [NodeCategory("Parasite.ReceiveData")]

    [OutPortNames("Report")]
    [OutPortDescriptions("OutToolTip0")]
    [OutPortTypes("Report")]
    public class ParasiteReceiver : VariableInputNode//: IReceiveData
    {
        private ParasiteReceiver()
        {
 

        }


   

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<object> ReceiveData(string id)
        {
            List<object> outPut = new List<object>();

            RequestData rd = new RequestData();

            DataContainer dataContainer = rd.RequestDataLocal(id);

            for (int i = 0; i < dataContainer.Data.Length; i++)
            {
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
            }

            return outPut;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override string GetInputName(int index)
        {
            return "Data" + index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override string GetInputTooltip(int index)
        {
            return "Foo" + index;
        }

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
    

  


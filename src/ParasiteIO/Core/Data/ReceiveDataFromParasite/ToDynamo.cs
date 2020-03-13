
//using Parasite.Conversion.Dynamo;
//using Parasite.Core.Collections;
//using Parasite.Core.Exceptions;
//using Parasite.Core.Sync;
//using Parasite.Core.Types.Geometry;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parasite.Core.Data.ReceiveDataFromParasite
//{
//    public class ToDynamo : IReceiveData
//    {
//        public List<object> Receive(string ID)
//        {
//            List<object> outPut = new List<object>();

//            RequestData rd = new RequestData();


//            DataContainer dataContainer = rd.RequestDataLocal(id);

//            foreach (var item in dataContainer.Data)
//            {
//                if (item.Value.Node is Parasite_Mesh mesh)
//                {
//                    if (mesh.VertexColors == null)
//                        outPut.Add(DynamoConversion.ToDynamoType(mesh));
//                    else
//                    {
//                        Mesh m = DynamoConversion.ToDynamoType(mesh);

//                        MeshWrapper wrapper = new MeshWrapper(m.VertexNormals, m.VertexPositions, mesh.VertexColors);

//                        outPut.Add(wrapper);
//                    }

//                }

//                else if (item.Value.Node is Parasite_NurbsCurve nurbsCurve)
//                    outPut.Add(DynamoConversion.ToDynamoType(nurbsCurve));

//                else if (item.Value.Node is Parasite_BrepSurface brepSrf)
//                    outPut.Add(DynamoConversion.ToDynamoType(brepSrf));
//                else
//                    throw new ParasiteNotImplementedExceptions("Type conversion not implemented yet!");


//            }

//            return outPut;
//        }
//    }
//}

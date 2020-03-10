using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parasite.Core.Sync;


using Parasite.Core.Types.Geometry;
using Parasite.Core.Data;
using Parasite.Conversion.Dynamo;
using Parasite.Core.Exceptions;

using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

using Parasite.Dynamo.Wrappers.Geometry;

namespace Parasite.Dynamo
{
    public class GetData
    {
        private GetData()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<object> Receive(string id)
        {
            List<object> outPut = new List<object>();

            RequestData rd = new RequestData();

            try
            {
                DataContainer dataContainer = rd.RequestDataLocal(id);

                foreach (var item in dataContainer.Data)
                {
                    if (item.Value.Node is Parasite_Mesh mesh)
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

                    else if (item.Value.Node is Parasite_NurbsCurve nurbsCurve)
                        outPut.Add(DynamoConversion.ToDynamoType(nurbsCurve));

                    else if (item.Value.Node is Parasite_BrepSurface brepSrf)
                        outPut.Add(DynamoConversion.ToDynamoType(brepSrf));
                    else
                        throw new ParasiteNotImplementedExceptions("Type conversion not implemented yet!");


                }



            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return outPut;
        }

    }

    public class DeconstructMesh
    {
        private DeconstructMesh() { }






        [MultiReturn(new[] { "Vertices", "FaceIndexes", "Edges", "Normals" })]
        public static Dictionary<string, object> MeshDeconstruct(Mesh mesh)
        {
            List<List<uint>> faceIndexes = new List<List<uint>>();
            IndexGroup[] ig = mesh.FaceIndices;
            Point[] vertices = mesh.VertexPositions;

            List<PolyCurve> edges = new List<PolyCurve>();

            for (int i = 0; i < ig.Length; i++)
            {
                if (ig[i].Count == 3)
                    faceIndexes.Add(new List<uint> { ig[i].A, ig[i].B, ig[i].C });


                if (ig[i].Count == 4)
                    faceIndexes.Add(new List<uint> { ig[i].A, ig[i].B, ig[i].C, ig[i].D });

            }

            for (int i = 0; i < faceIndexes.Count; i++)
            {
                for (int j = 0; j < faceIndexes[i].Count; j++)
                {
                    if (faceIndexes[i].Count == 3)
                    {
                        Point[] verts = new Point[3];
                        verts[0] = vertices[(int)faceIndexes[i][0]];
                        verts[1] = vertices[(int)faceIndexes[i][1]];
                        verts[2] = vertices[(int)faceIndexes[i][2]];

                        edges.Add(PolyCurve.ByPoints(verts));

                    }

                    if (faceIndexes[i].Count == 4)
                    {
                        Point[] verts = new Point[4];

                        verts[0] = vertices[(int)faceIndexes[i][0]];
                        verts[1] = vertices[(int)faceIndexes[i][1]];
                        verts[2] = vertices[(int)faceIndexes[i][2]];
                        verts[3] = vertices[(int)faceIndexes[i][3]];

                        edges.Add(PolyCurve.ByPoints(verts));
                    }
                }

            }



            return new Dictionary<string, object>
            {
                { "vertices",mesh.VertexPositions},
                { "faceIndexes",faceIndexes},
                {"Edges", edges },
                {"Normals", mesh.VertexNormals }

            };
        }


    }
}

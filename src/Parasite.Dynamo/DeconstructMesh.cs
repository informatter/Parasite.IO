using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Parasite.Dynamo.Nodes
{
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
                { "Vertices",mesh.VertexPositions},
                { "FaceIndexes",faceIndexes},
                {"Edges", edges },
                {"Normals", mesh.VertexNormals }

            };
        }


    }
}
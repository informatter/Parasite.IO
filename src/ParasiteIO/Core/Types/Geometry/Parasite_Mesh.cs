using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using ParasiteIO.Core.Exceptions;
using ParasiteIO.Conversion.Parasite;
using ParasiteIO.Core.Data.Parameter;

namespace ParasiteIO.Core.Types.Geometry
{

    [Serializable]
    public class Parasite_Mesh : ParasiteAbstractObject
    {
        #region FIELDS   
         int[][] m_faceIndexes;
         Parasite_Point3d[] m_vertices;
         Color[] m_vertexColors;


        #endregion
        public Parasite_Mesh(int[][] faceIndexes, Parasite_Point3d[] vertices, Color[]  vertexColors = null, Dictionary<string, Parameter> properties = null) : base(properties)
        {
            base.Properties = properties;
            base.TypeName = GetType().Name;        
            m_faceIndexes = faceIndexes;
            m_vertices = vertices;
            m_vertexColors = vertexColors;

        }



        #region PROPERTIES

        public Color[] VertexColors { get => m_vertexColors; }
        public int[][] FaceIndexes { get => m_faceIndexes; }
        public Parasite_Point3d[] Vertices { get => m_vertices; }

        public bool IsValid { get => CheckValidity(); }




        private bool CheckValidity()
        {
            for (int i = 0; i < FaceIndexes.Length; i++)
            {
                for (int j = 0; j < FaceIndexes[i].Length; j++)
                {
                    if (FaceIndexes[i].Length == 3)
                    {
                        Parasite_Point3d A = Vertices[FaceIndexes[i][0]];
                        Parasite_Point3d B = Vertices[FaceIndexes[i][1]];
                        Parasite_Point3d C = Vertices[FaceIndexes[i][2]];

                        Parasite_Point3d AB = B - A;
                        Parasite_Point3d AC = C - A;

                        Parasite_Point3d crossP = Parasite_Point3d.CrossProduct(AB, AC);

                        double area = 0.5 * crossP.Magnitude;

                        if (area <= 0) return false;

                    }

                    if (FaceIndexes[i].Length == 4)
                    {
                        Parasite_Point3d A = Vertices[FaceIndexes[i][0]];
                        Parasite_Point3d B = Vertices[FaceIndexes[i][1]];
                        Parasite_Point3d C = Vertices[FaceIndexes[i][2]]; // I think I dont need this point
                        Parasite_Point3d D = Vertices[FaceIndexes[i][3]];

                        Parasite_Point3d AB = B - A;
                        Parasite_Point3d AD = D - A;

                        double area = AB.Magnitude * AD.Magnitude;

                        if (area <= 0) return false;

                    }

                    if (FaceIndexes[i].Length < 3 || FaceIndexes[i].Length > 4) return false;

                }
            }

            return true;
        }




        #endregion


    }


}

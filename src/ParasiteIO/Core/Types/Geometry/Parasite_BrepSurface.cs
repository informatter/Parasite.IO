

using System;
using System.Collections.Generic;


namespace Parasite.Core.Types.Geometry
{
  
    [Serializable]
    public class Parasite_BrepSurface: ParasiteObject
    {
    


        #region FIELDS   

   
        IEnumerable<Parasite_Point3d> m_vertices;
        IEnumerable<Parasite_Line> m_edges;


        #endregion

        #region CONSTRUCTORS

        public Parasite_BrepSurface(IEnumerable<Parasite_Point3d> vertices, IEnumerable<Parasite_Line> edges =null, Dictionary<string, string> properties = null) : base(properties)
        {
            base.Properties = properties;
            base.TypeName = GetType().Name;
            m_vertices = vertices;
            m_edges = edges;

        }


        #endregion

        #region PROPERTIES
        public IEnumerable<Parasite_Point3d> Vertices { get => m_vertices; }
        public IEnumerable<Parasite_Line> Edges { get => m_edges; }



        #endregion





        #region EXPLICIT INTERFACE IMPLEMENTATION
        #endregion

        #region NESTED CLASSES
        [Serializable]
        public class Parasite_Cylinder : ParasiteObject
        {

            public Parasite_Cylinder()
            {

            }


            #region PROPERTIES
            public int  Height1 { get; }
            public int Height2 { get; }

            #endregion


        }
        #endregion
    }
}

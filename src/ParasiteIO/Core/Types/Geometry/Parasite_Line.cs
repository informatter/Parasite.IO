using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParasiteIO.Core.Types;

namespace ParasiteIO.Core.Types.Geometry
{
    [Serializable]
    public class Parasite_Line : ParasiteAbstractObject
    {
        private Parasite_Point3d m_startPt;
        private Parasite_Point3d m_endPt;



        public Parasite_Line(Parasite_Point3d startPt, Parasite_Point3d endPt, Dictionary<string, string> properties = null) : base(properties)
        {
            base.Properties = properties;
            base.TypeName = GetType().Name;
            m_startPt = startPt;
            m_endPt = endPt;
        }

        public Parasite_Point3d EndPoint {get => m_startPt; }
        public Parasite_Point3d StartPoint { get => m_endPt; }
        public Parasite_Point3d[] Vertices { get => new Parasite_Point3d[] { m_startPt, m_endPt }; }

    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parasite.Core.Types.Display;

namespace Parasite.Core.Types.Geometry
{
    public class Parasite_Sphere: ParasiteObject
    {

        Parasite_Point3d m_center;
        double m_radius;
        Parasite_Color m_color;
        public Parasite_Sphere(Parasite_Point3d center, double radius, Parasite_Color color = null, Dictionary<string, string> properties = null):base(properties)
        {
            base.Properties = properties;
            base.TypeName = GetType().Name;
            m_color = color;
            m_center = center;
            m_radius = radius;
        }

        public Parasite_Point3d Center { get => m_center; }
        public double Radius { get => m_radius; }

        public Parasite_Color Color { get => m_color; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Core.Types.Display
{

    /// <summary>
    /// loose Re-implementation of the System.Drawing.Color class in .NET
    /// </summary>
    
    [Serializable]
    public class Parasite_Color
    {
        private double m_a;
        private double m_r;
        private double m_g;
        private double m_b;

     

        public Parasite_Color(double A ,double R, double G, double B)
        {
            this.m_r = (R > 255) ? 255 : ((R < 0) ? 0 : R);
            this.m_g = (G > 255) ? 255 : ((G < 0) ? 0 : G);
            this.m_b = (B > 255) ? 255 : ((B < 0) ? 0 : B);
            this.m_a = (A > 255) ? 255 : ((A < 0) ? 0 : A);
        }


        public Parasite_Color()
        {
            
        }

        public double A { get => m_a; }
        public double R { get => m_r; }
        public double G { get => m_g; }
        public double B { get => m_b; }

        public static Parasite_Color Invalid() => new Parasite_Color(double.NaN, double.NaN, double.NaN, double.NaN);

    }
}

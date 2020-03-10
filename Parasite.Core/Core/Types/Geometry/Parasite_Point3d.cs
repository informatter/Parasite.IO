using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Core.Types.Geometry
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [Serializable]
    public class Parasite_Point3d: ParasiteObject
    {


        double m_x;
        double m_y;
        double m_z;

        public Parasite_Point3d(double x, double y, double z, Dictionary<string, string> properties =null )
        {
            m_x = x;
            m_y = y;
            m_z = z;
            base.Properties = properties;
            base.TypeName = GetType().Name;       
        }



        public double X { get => m_x; }
        public double Y { get => m_y; }
        public double Z { get => m_z; }

        public double Magnitude { get => Math.Sqrt(m_x * m_x + m_y * m_y + m_z * m_z); }


        public static Parasite_Point3d operator -(Parasite_Point3d vecA, Parasite_Point3d vecB)
        {
           double x =  vecA.m_x - vecB.m_x;
           double y = vecA.m_y - vecB.m_y;
           double z = vecA.m_z - vecB.m_x;
           return new Parasite_Point3d(x,y,z);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Parasite_Point3d operator -(Parasite_Point3d vec)
        {
            double x =  vec.m_x -vec.m_x;
            double y = vec.m_y - vec.m_y;
            double z = vec.m_z- vec.m_z;
            return new Parasite_Point3d(x, y, z);
        }


        public static Parasite_Point3d CrossProduct(Parasite_Point3d vecA, Parasite_Point3d vecB)
        {
            //John Vince, Mathematics for computer graphics
            // t = VecA X VecB
            // |t| = |VecA| |VecB| Sin(theta)

            // determinant multiplication
            double deltaX = (vecA.m_y * vecB.m_z) - (vecA.m_z * vecB.m_y);
            double deltaY = (vecA.m_z * vecB.m_x) - (vecA.m_x * vecB.m_z);
            double deltaZ = (vecA.m_x * vecB.m_y) - (vecA.m_y * vecB.m_x);
            return new Parasite_Point3d(deltaX, deltaY, deltaZ);
        }

    
  


        public static Parasite_Point3d Invalid()=> new Parasite_Point3d(double.NaN, double.NaN, double.NaN);



    }


}

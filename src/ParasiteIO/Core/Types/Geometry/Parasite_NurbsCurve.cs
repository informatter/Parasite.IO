using Parasite.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Core.Types.Geometry
{

    [Serializable]
    public class Parasite_NurbsCurve : ParasiteObject
    {
        #region FIELDS
        private Parasite_Point3d[] m_controlPoints;
        private int m_degree;
        double[] m_interiorKnotMultiplicity;
        private double[] m_knots;
        private double[] m_weights;
        #endregion


        #region PROPERTIES

        public Parasite_Point3d [] ControlPoints { get => m_controlPoints; }
        public int Degree { get => m_degree; }
        public double[] InteriorKnotMultiplicity { get => m_interiorKnotMultiplicity; }
        public double [] Knots { get => m_knots; }
        public double[] Weights { get => m_weights; }

        #endregion


        /// <summary>
        /// Constructs a Parasite Nurbs Curve from given parameters
        /// </summary>
        /// <param name="controlPoints"></param>
        /// <param name="weights"></param>
        /// <param name="knots"></param>
        /// <param name="degree"></param>
        /// <param name="properties"></param>
        public Parasite_NurbsCurve(Parasite_Point3d [] controlPoints, double [] weights,double[] knots,double [] interiorKnotMultiplicity, 
            int degree, Dictionary<string, string> properties = null)
        {
            base.Properties = properties;
            base.TypeName = GetType().Name;
            m_controlPoints = controlPoints;
            m_weights = weights;
            m_knots = knots;
            m_degree = degree;
            m_interiorKnotMultiplicity = interiorKnotMultiplicity;

        }

     
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Geometry;
using Rhino.Geometry.Collections;

using Parasite.Core.Exceptions;
using Autodesk.DesignScript.Geometry;

namespace Parasite.Core.Types.Geometry
{

    [Serializable]
    public class Parasite_NurbsSurface: ParasiteObject
    {

        #region FIELDS
        private int m_degreeU;
        private int m_degreeV;
        private double [] m_knotsU;
        private double[] m_knotsV;
        private Parasite_Point3d [][] m_controlPoints;
        private double[][] m_weights;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nurbsSurface"></param>
        /// <param name="properties"></param>
        public Parasite_NurbsSurface(Parasite_Point3d[][] controlPoints,double [] knotsU, double[] knotsV, double[][] weights,
           int  degreeU, int degreeV,  Dictionary<string, string> properties = null)
        {
            base.Properties = properties;
            base.TypeName = GetType().Name;
            m_degreeU = degreeU;
            m_degreeV = degreeV;
            m_knotsU = knotsU;
            m_knotsV = knotsV;
            m_controlPoints = controlPoints;
            m_weights = weights;


        }

        #endregion

        #region PROPERTIES

        public int DegreeU { get => m_degreeU; }
        public int DegreeV { get => m_degreeV; }
        public double [] KnotsU { get => m_knotsU; }
        public double[] KnotsV { get => m_knotsV; }

        public Parasite_Point3d[][] ControlPoints { get => m_controlPoints; }
        public double [][] Weights { get => m_weights; }



        #endregion


        #region METHODS


        


        #endregion


    }


}

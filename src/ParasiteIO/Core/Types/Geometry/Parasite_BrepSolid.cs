
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parasite.Conversion.Parasite;
using Parasite.Core.Exceptions;
using Rhino.Geometry;
using Rhino.Geometry.Collections;

namespace Parasite.Core.Types.Geometry
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Parasite_BrepSolid : ParasiteObject
    {
        public enum ParasiteBrepTypes { Cylinder, BrepSolid };

        #region FIELDS 

        ParasiteBrepTypes m_parasiteBrepTypes;
        Parasite_BrepSurface[] m_faces;
        #endregion

        #region CONSTRUCTORS

        public Parasite_BrepSolid(Parasite_BrepSurface[] faces, Dictionary<string, string> properties = null)
        {
            base.Properties = properties;
            base.TypeName = GetType().Name;
            m_faces = faces;
        }
        #endregion

        #region PROPERTIES
        public Parasite_BrepSurface[] Faces { get => m_faces; }

        #endregion

        #region METHODS


        #endregion



    }
}

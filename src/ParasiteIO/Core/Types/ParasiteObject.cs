
using System;
using System.Collections.Generic;

using ParasiteIO.Core.Types;
using ParasiteIO.Core.Data.Properties;

namespace ParasiteIO.Core.Types
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ParasiteObject: ParasiteAbstractObject
    {
        object m_data;

        Property m_property;

        public ParasiteObject(object data, Property property)
        {
            m_data = data; m_property = property;
        }

        public object Data { get => m_data; set => m_data = value; }

        public Property Property { get => m_property; }
    }
}

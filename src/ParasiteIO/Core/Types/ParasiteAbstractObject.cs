﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParasiteIO.Core.Types
{
    [Serializable]
    public abstract class ParasiteAbstractObject: IParasiteObject
    {
        private string m_typeName;
       private Dictionary<string, string> m_properties;

        public ParasiteAbstractObject(Dictionary<string, string> properties = null)
        {
            m_properties = properties;
        }

       

        public string TypeName { get => m_typeName; set => m_typeName = value; }

      

        public Dictionary<string, string> Properties
        {
            get { return m_properties; }
            set { m_properties = value; }
        }

    

    }
}

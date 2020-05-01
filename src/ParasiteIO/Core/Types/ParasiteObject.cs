
using System;
using System.Collections.Generic;

using ParasiteIO.Core.Types;
using ParasiteIO.Core.Data.Properties;
using System.Runtime.CompilerServices;
using ParasiteIO.Core.Data.Parameter;

namespace ParasiteIO.Core.Types
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ParasiteObject: ParasiteAbstractObject
    {
        object m_data;

        public ParasiteObject() { }
        public ParasiteObject(object data, Dictionary<string, Parameter> properties = null):base(properties)
        {
            if (properties == null)            
                base.Properties = new Dictionary<string, Parameter>();           
            else
                base.Properties = properties;


            m_data = data; 
        }

        public object Data { get => m_data; set => m_data = value; }



        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        public override void AddParameter(string name, Parameter parameter)
        {
            base.AddParameter(name, parameter);
        }


        /// <summary>
        /// Returns a parameter by its name. 
        /// Returns false if not succesfull
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool GetParameter(string name, out Parameter value)
        {
            return base.GetParameter(name, out value);
        }
    }
}

using ParasiteIO.Core.Data.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParasiteIO.Core.Types
{
    [Serializable]
    public abstract class ParasiteAbstractObject: IParasiteAbstractObject
    {
        private string m_typeName;
       private Dictionary<string, Parameter> m_properties;

        public ParasiteAbstractObject(Dictionary<string, Parameter> properties = null)
        {
            if (properties == null)
            {
                m_properties = new Dictionary<string, Parameter>();
            }

            else
                m_properties = properties;
        }

       

        public string TypeName { get => m_typeName; set => m_typeName = value; }

      

        public Dictionary<string, Parameter> Properties
        {
            get { return m_properties; }
            set { m_properties = value; }
        }

        Dictionary<string, Parameter> IParasiteAbstractObject.Properties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public virtual void AddParameter(string name, Parameter parameter)
        {
            m_properties.Add(name, parameter);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nam"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool GetParameter(string name, out Parameter value)
        {
            bool success;
            if (Properties.TryGetValue(name, out Parameter _value)) success = true;
            else
                success = false;


            value = _value;
            return success;
        }
    }
}

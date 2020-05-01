using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParasiteIO.Core.Data.Parameter
{
    [Serializable]
    public class Parameter
    {
        ParameterType m_parameterType;
        dynamic m_value;
        StorageType m_storageType;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="storageType"></param>
        public Parameter(ParameterType parameterType, dynamic value, StorageType storageType)
        {
            m_parameterType = parameterType;
            m_value = value;
            m_storageType = storageType;
        }


        public ParameterType ParameterType { get => m_parameterType; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic GetValue()
        {
            dynamic output = null;

            switch (m_storageType)
            {
                case StorageType.Double:
                    {
                        double data = AsDouble();
                        output = data;
                        break;
                    }

                case StorageType.Integer:
                    {
                        int data = AsInteger();
                        output = data;
                        break;
                    }

                case StorageType.String:
                    {
                        string data = AsString();
                        output = data;
                        break;
                    }
            }

            return output;
        }

        private double AsDouble()
        {
            if(m_value is double)
            {
                return (double) m_value;
            }

            return -1.0;
        }

        private int AsInteger()
        {
            if (m_value is int)
            {
                return (int)m_value;
            }

            return -1;
        }

        private string AsString()
        {
            if (m_value is string)
            {
                return (string)m_value;
            }

            return null;
        }

    }
}

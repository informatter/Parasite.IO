using ParasiteIO.Core.Data.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParasiteIO.Core.Types
{

   
    public interface IParasiteAbstractObject
    {
        string TypeName { get; }
        Dictionary<string,Parameter> Properties { get; set; }

       void AddParameter(string key, Parameter parameter);

        bool GetParameter(string key, out Parameter value);
    
     
    }
}

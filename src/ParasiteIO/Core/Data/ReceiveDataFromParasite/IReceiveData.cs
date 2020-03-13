using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Core.Data.ReceiveDataFromParasite
{
    public interface IReceiveData
    {
         List<object> ReceiveData(string ID);
    }
}

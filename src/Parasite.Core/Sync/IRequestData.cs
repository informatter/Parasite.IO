using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parasite.Core.Data;

namespace Parasite.Core.Sync
{
    public interface IRequestData 
    {
         DataContainer RequestDataLocal (string fileName);
         DataContainer RequestDataServer(string fileName);

    }
}

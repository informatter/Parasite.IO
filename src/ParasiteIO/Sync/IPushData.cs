using Parasite.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Parasite.Core.Collections;

namespace Parasite.Core.Sync
{
    public interface IPushData
    {
        void PushDataLocal(DataContainer data, string fileName);
        void PushDataServerAsync(DataContainer data, string url);
        void PushDataServer(DataContainer data, string url);
    }
}

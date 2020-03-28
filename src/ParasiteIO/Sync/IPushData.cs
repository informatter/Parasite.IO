
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParasiteIO.Core.Data;
using ParasiteIO.Core.Collections;

namespace ParasiteIO.Core.Sync
{
    public interface IPushData
    {
        void PushDataLocal(DataContainer data, string fileName);
        void PushDataServerAsync(DataContainer data, string url);
        void PushDataServer(DataContainer data, string url);
    }
}


using System;
using System.Collections.Generic;


using ParasiteIO.Core.Collections;

namespace ParasiteIO.Core.Data.CollectDataFromApplication
{
    public interface IApplicationDataCollector
    {
        DataContainer CollectData(List<List<object>> dataFromApp);
    }
}

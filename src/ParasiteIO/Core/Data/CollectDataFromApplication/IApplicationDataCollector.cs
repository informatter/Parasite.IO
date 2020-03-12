using Parasite.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Core.Data.CollectDataFromApplication
{
    public interface IApplicationDataCollector
    {
        DataContainer CollectDataFromApplication(List<DataContainerFactory> dataFromApp);
    }
}


using ParasiteIO.Core.Collections;

namespace ParasiteIO.Core.Sync
{
    public interface IRequestData 
    {
         DataContainer RequestDataLocal (string fileName);
         DataContainer RequestDataServer(string fileName);

    }
}

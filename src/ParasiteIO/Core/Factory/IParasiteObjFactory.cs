using ParasiteIO.Core.Data.Properties;
using ParasiteIO.Core.Types;
using System;

namespace ParasiteIO.Core.Factory
{
    public interface IParasiteObjFactory
    {

        IParasiteAbstractObject CreateParasiteObject(IProperty property, object data);
    }
}
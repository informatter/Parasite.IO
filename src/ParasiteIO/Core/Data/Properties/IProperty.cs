using ParasiteIO.Core.Types;

namespace ParasiteIO.Core.Data.Properties
{
    public interface IProperty
    {
        ParasiteCategories Category { get; }
        string Name { get; }
    }
}
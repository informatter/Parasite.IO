
using System;
using ParasiteIO.Core.Types;

namespace ParasiteIO.Core.Data.Properties
{

    /// <summary>
    /// This class is still under design
    /// its very likely that it will change.
    /// The initial intention for it is to be able to import Revit geometry with their respective types. Walls, frames, glass, etc.
    /// </summary>
    [Serializable]
    public class Property
    {
        ParasiteCategories m_category;
        string m_name;

        public Property(ParasiteCategories category, string name) { m_category = category; m_name = name; }

        
        /// <summary>
        /// The category of this property. i.e walls, frame, mullions... ect
        /// </summary>
        public ParasiteCategories Category { get => m_category; }

        public string Name { get => m_name; }
    }
}

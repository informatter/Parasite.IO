using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ParasiteIO.Core.Types;
using ParasiteIO.Conversion.Parasite;
using ParasiteIO.Core.Exceptions;
using ParasiteIO.Core.Data.Parameter;

namespace ParasiteIO.Core.Types.Geometry
{
    
    [Serializable]
    public class Parasite_Polyline: ParasiteAbstractObject
    {

        private Parasite_Point3d[] m_vertices;
        private bool m_isClosed;
        public Parasite_Polyline(object polyline, Dictionary<string, Parameter> properties =null):base(properties)
        {
            base.Properties = properties;
            base.TypeName = GetType().Name;
            Foo(polyline);
        }


        public Parasite_Point3d [] Vertices { get => m_vertices; }
        public bool  Closed { get => m_isClosed; }

        protected  void Foo(object geo)
        {
           if(geo is Rhino.Geometry.Polyline)
            {
               
                Rhino.Geometry.Polyline polyline = geo as Rhino.Geometry.Polyline;

                if (!polyline.IsValid)
                    throw new ParasiteArgumentException("Please input a valid Rhino Polyline");

                if (polyline.IsClosed) m_isClosed = true;

               //  m_vertices =  polyline.Select(a => ToParasiteType(a)).ToArray();

            }

           else
                throw new ParasiteNotImplementedExceptions("Type conversion not implemented yet!");

        }
    }
}

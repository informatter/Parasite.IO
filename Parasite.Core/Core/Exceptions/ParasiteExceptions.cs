using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Core.Exceptions
{
    public class ParasiteAppTypeExceptions:Exception
    {
        public ParasiteAppTypeExceptions(Type type) :
         this(type, "")
        { }

        public ParasiteAppTypeExceptions(Type type, string msg) :
           base(string.Format("Implementation handling for Type of {0} does not exist still", type, msg))
        { }

     
    }

    public class ParasiteConversionExceptions : Exception
    {
        public ParasiteConversionExceptions(Type from, Type to) :
      this(from,to,"")
        { }

        public ParasiteConversionExceptions(Type from, Type to, string msg) : 
            base(string.Format("Could not convert from {0} to {1}", from, to,msg))
        { }
        
    }


    public class ParasiteNotImplementedExceptions: Exception
    {
    
        public ParasiteNotImplementedExceptions(string msg) :
        base(msg)
        { }
    }




    public class ParasiteArgumentException : Exception
    {
        public ParasiteArgumentException() :
      this("")
        { }

        public ParasiteArgumentException(string msg) :
            base(  msg)
        { }

    }
}

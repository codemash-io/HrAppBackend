using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class BussinessException : Exception
    {
        public BussinessException() { }

        public BussinessException(string message) : base(message) { }

        public BussinessException(string message, Exception inner) : base(message, inner) { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Aot.Hrms.Contracts
{
    public class InsertFailedException : Exception
    {
        public InsertFailedException(string message) : base(message)
        {

        }
    }
}

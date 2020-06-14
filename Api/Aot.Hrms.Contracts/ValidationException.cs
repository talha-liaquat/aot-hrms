using System;
using System.Collections.Generic;
using System.Text;

namespace Aot.Hrms.Contracts
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {

        }
    }
}

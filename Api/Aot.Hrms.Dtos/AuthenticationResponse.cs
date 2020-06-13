using System;
using System.Collections.Generic;
using System.Text;

namespace Aot.Hrms.Dtos
{
    public class AuthenticationResponse
    {
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public string Token { get; set; }
    }
}

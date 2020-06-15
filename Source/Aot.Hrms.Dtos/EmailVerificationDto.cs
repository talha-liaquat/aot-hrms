using System;
using System.Collections.Generic;
using System.Text;

namespace Aot.Hrms.Dtos
{
    public class EmailVerificationDto
    {
        public string EmployeeId { get; set; }
        public string Email { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}

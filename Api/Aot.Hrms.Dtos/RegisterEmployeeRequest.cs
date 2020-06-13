using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aot.Hrms.Dtos
{
    public class RegisterEmployeeRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class EmployeeDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public string Id { get; set; }
        public bool IsActive { get; set; }
    }

    public class VerifyEmployeeRequest
    {
        [Required]
        public string EmployeeId { get; set; }
    }

    public class RegisterSkillRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string CreatedBy { get; set; }
    }

    public class UnAssignSkillRequest
    {
        [Required]
        public string EmployeeSkillId { get; set; }
    }
}

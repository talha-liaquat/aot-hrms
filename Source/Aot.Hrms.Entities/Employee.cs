using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Aot.Hrms.Entities
{
    public class Employee : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Index(IsUnique = true)]
        public string Email { get; set; }
        [Required]
        public bool IsEmailVerified { get; set; }
        public bool IsAdmin { get; set; }
        public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; }
    }
}
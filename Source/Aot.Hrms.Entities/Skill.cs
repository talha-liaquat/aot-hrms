using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aot.Hrms.Entities
{
    public class Skill : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aot.Hrms.Entities
{
    public class EmployeeSkill : BaseEntity
    {
        [Required]
        public string EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        [Required]
        public string SkillId { get; set; }
        public virtual Skill Skill { get; set; }
    }
}

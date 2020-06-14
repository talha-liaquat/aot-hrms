using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Index = Microsoft.EntityFrameworkCore.Metadata.Internal.Index;

namespace Aot.Hrms.Entities
{
    public class User : BaseEntity
    {
        [StringLength(1000)]
        public string AutoToken { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        [Index(IsUnique = true)]
        public string Username { get; set; }

        [Required]
        [StringLength(250)]
        public string Password { get; set; }

        [Required]
        [StringLength(250)]
        public string Email { get; set; }
        [Required]
        public string EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}

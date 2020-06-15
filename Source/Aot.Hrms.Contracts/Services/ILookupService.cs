using Aot.Hrms.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aot.Hrms.Contracts.Services
{
    public interface ILookupService
    {
        public IList<SkillDto> GetAllSkills();
        public Task<SkillDto> CreateSkillAsync(RegisterSkillRequest request);
    }
}

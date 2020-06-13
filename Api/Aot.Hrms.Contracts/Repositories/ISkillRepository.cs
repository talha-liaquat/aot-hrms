using Aot.Hrms.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aot.Hrms.Contracts.Repositories
{
    public interface ISkillRepository
    {
        public Task<int> CreateAsync(Entities.Skill skill);
        public IList<Skill> GetAll();
    }
}

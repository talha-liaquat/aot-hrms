using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aot.Hrms.Contracts.Repositories
{
    public interface IEmployeeSkillRepository
    {
        public Task<int> CreateAsync(Entities.EmployeeSkill employeeSkill);
        public Task<int> CreateManyAsync(List<Entities.EmployeeSkill> employeeSkills);
        public Task<int> UpdateAsync(Entities.EmployeeSkill employeeSkill);
        public Entities.EmployeeSkill GetById(string id);
        public IList<Entities.EmployeeSkill> GetByEmployeeId(string employeeId);
        public IList<Entities.EmployeeSkill> GetBySkillId(string skillId);
    }
}

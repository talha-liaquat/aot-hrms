using Aot.Hrms.Contracts.Repositories;
using Aot.Hrms.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aot.Hrms.Repositories
{
    public class EmployeeSkillRepository : IEmployeeSkillRepository
    {
        public async Task<int> CreateAsync(EmployeeSkill employeeSkill)
        {
            using var context = new AotDBContext();
            await context.EmployeeSkills.AddAsync(employeeSkill);
            return await context.SaveChangesAsync();
        }

        public async Task<int> CreateManyAsync(List<Entities.EmployeeSkill> employeeSkills)
        {
            using var context = new AotDBContext();
            foreach (var employeeSkill in employeeSkills)
            {
                await context.EmployeeSkills.AddAsync(employeeSkill);
            }
            return await context.SaveChangesAsync();
        }

        public IList<EmployeeSkill> GetByEmployeeId(string employeeId)
        {
            using var context = new AotDBContext();
            return (from empSkills in context.EmployeeSkills
            join skl in context.Skill on empSkills.SkillId equals skl.Id
            where empSkills.EmployeeId == employeeId
            select new EmployeeSkill
            {
                Id = empSkills.Id,
                Skill = skl                
            })?.ToList();
        }

        public EmployeeSkill GetById(string id)
        {
            using var context = new AotDBContext();
            return context.EmployeeSkills.SingleOrDefault(x => x.Id == id && x.IsActive);
        }

        public IList<EmployeeSkill> GetBySkillId(string skillId)
        {
            using var context = new AotDBContext();
            return (from empSkills in context.EmployeeSkills
                    join emp in context.Employee on empSkills.EmployeeId equals emp.Id
                    where empSkills.SkillId == skillId
                    select new EmployeeSkill
                    {
                        Id = empSkills.Id,
                        Employee = emp
                    })?.ToList();
        }

        public async Task<int> UpdateAsync(EmployeeSkill employeeSkill)
        {
            using var context = new AotDBContext();
            context.EmployeeSkills.Update(employeeSkill);
            return await context.SaveChangesAsync();
        }
    }
}

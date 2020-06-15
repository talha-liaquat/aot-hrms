using Aot.Hrms.Contracts.Repositories;
using Aot.Hrms.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Aot.Hrms.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        public async Task<int> CreateAsync(Skill skill)
        {
            using var context = new AotDBContext();
            await context.Skill.AddAsync(skill);
            return await context.SaveChangesAsync();
        }

        public IList<Skill> GetAll()
        {
            var skills = new List<Skill>();
            using var context = new AotDBContext();
            skills = context.Skill.ToList();
            return skills;
        }

        public Skill GetSkillByTitle(string title)
        {
            using var context = new AotDBContext();
            return context.Skill.SingleOrDefault(x => x.Title.ToLower() == title.ToLower());
        }
    }
}

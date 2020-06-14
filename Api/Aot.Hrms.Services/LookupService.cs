using Aot.Hrms.Contracts;
using Aot.Hrms.Contracts.Repositories;
using Aot.Hrms.Contracts.Services;
using Aot.Hrms.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aot.Hrms.Services
{
    public class LookupService : ILookupService
    {
        private readonly ISkillRepository _skillRepository;

        public LookupService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository ?? throw new ArgumentNullException(nameof(skillRepository));
        }

        public async Task<SkillDto> CreateSkillAsync(RegisterSkillRequest request)
        {
            var existingSkill = _skillRepository.GetSkillByTitle(request.Title);

            if (existingSkill != null)
                throw new InsertFailedException("Skill already added in the System!");

            var newSkill = new Entities.Skill
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = request.CreatedBy,
                Title = request.Title
            };

            await _skillRepository.CreateAsync(newSkill);

            return new SkillDto
            {
                Id = newSkill.Id,
                Title = newSkill.Title
            };
        }

        public IList<SkillDto> GetAllSkills()
        {
            return _skillRepository.GetAll().Select(x => new SkillDto { Id = x.Id, Title = x.Title })?.ToList();
        }
    }
}

using Aot.Hrms.Contracts.Repositories;
using Aot.Hrms.Contracts.Services;
using Aot.Hrms.Dtos;
using Aot.Hrms.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aot.Hrms.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeSkillRepository _employeeSkillRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IEmployeeSkillRepository employeeSkillRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _employeeSkillRepository = employeeSkillRepository ?? throw new ArgumentNullException(nameof(employeeSkillRepository));
        }

        public async Task<string> AssignSkillAsync(string employeeId, string skillId, string userId)
        {
            var employeeSkill = new EmployeeSkill
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreateOn = DateTime.UtcNow,
                IsActive = true,
                SkillId = skillId,
                EmployeeId = employeeId,
            };

            await _employeeSkillRepository.CreateAsync(employeeSkill);

            return employeeSkill.Id;
        }

        public async Task<EmployeeDto> RegisterAsync(RegisterEmployeeRequest request)
        {
            var id = Guid.NewGuid().ToString();
            var employee = new Employee
            {
                Id = id,
                CreatedBy = id,
                CreateOn = DateTime.UtcNow,
                IsActive = true,
                Email = request.Email,
                Name = request.Name,
                IsAdmin = request.IsAdmin,
                IsEmailVerified = false
            };

            await _employeeRepository.CreateAsync(employee);
            
            return MapEmployeeDto(employee);
        }

        private static EmployeeDto MapEmployeeDto(Employee employee)
        {
            return new EmployeeDto
            {
                Email = employee.Email,
                Id = employee.Id,
                IsActive = employee.IsActive,
                IsEmailVerified = employee.IsEmailVerified,
                Name = employee.Name
            };
        }

        public async Task<string> UnAssignSkillAsync(UnAssignSkillRequest request)
        {
            var empoyeeSkill = _employeeSkillRepository.GetById(request.EmployeeSkillId);
            empoyeeSkill.IsActive = false;
            await _employeeSkillRepository.UpdateAsync(empoyeeSkill);
            return empoyeeSkill.Id;
        }

        public async Task<EmployeeDto> VerifyAsync(VerifyEmployeeRequest request)
        {
            var employee = _employeeRepository.GetById(request.EmployeeId);
            employee.IsEmailVerified = true;
            await _employeeRepository.UpdateAsync(employee);
            return MapEmployeeDto(employee);
        }

        public IList<SkillDto> GetSkillsByEmployeeId(string employeeId)
        {
            return _employeeSkillRepository.GetByEmployeeId(employeeId)
                ?.Select(x => new SkillDto { Id = x.Skill.Id, Title = x.Skill.Title })
                ?.ToList();
        }

        public IList<EmployeeDto> GetEmployeeBySkillId(string skillId)
        {
            return _employeeSkillRepository.GetBySkillId(skillId)
              ?.Select(x => new EmployeeDto { 
                  Id = x.Employee.Id, 
                  Name = x.Employee.Name, 
                  Email = x.Employee.Email, 
                  IsActive = x.Employee.IsActive, 
                  IsEmailVerified = x.Employee.IsEmailVerified })
              ?.ToList();
        }
    }
}

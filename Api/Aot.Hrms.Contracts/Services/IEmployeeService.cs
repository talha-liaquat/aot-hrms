using Aot.Hrms.Dtos;
using Aot.Hrms.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aot.Hrms.Contracts.Services
{
    public interface IEmployeeService
    {
        public Task<EmployeeDto> RegisterAsync(RegisterEmployeeRequest request);
        public Task<EmployeeDto> VerifyAsync(VerifyEmployeeRequest request);
        public Task<string> AssignSkillAsync(string employeeId, string skillId, string userId);
        public IList<SkillDto> GetSkillsByEmployeeId(string employeeId);
        public IList<EmployeeDto> GetEmployeeBySkillId(string skillId);
        public Task<string> UnAssignSkillAsync(UnAssignSkillRequest request);
    }
}

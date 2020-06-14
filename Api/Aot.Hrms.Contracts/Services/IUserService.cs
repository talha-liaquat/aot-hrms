using Aot.Hrms.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aot.Hrms.Contracts.Services
{
    public interface IUserService
    {
        public Task<string> RegisterUserAsync(RegisterUserRequest request, string employeeId);
        public (string userId, string employeeId, bool isAdmin)? Authenticate(LoginRequest request);
    }
}

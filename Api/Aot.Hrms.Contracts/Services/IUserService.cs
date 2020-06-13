using Aot.Hrms.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aot.Hrms.Contracts.Services
{
    public interface IUserService
    {
        public Task<string> RegisterUserAsync(RegisterUserRequest request);
        public (string token, string userId)? Authenticate(LoginRequest request);
    }
}

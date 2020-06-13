using Aot.Hrms.Contracts;
using Aot.Hrms.Contracts.Repositories;
using Aot.Hrms.Contracts.Services;
using Aot.Hrms.Dtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Aot.Hrms.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public (string userId, string employeeId)? Authenticate(LoginRequest request)
        {
            var user = _userRepository.GetUser(request.Username, request.Password);

            if (user != null)
                return (user.Id, user.EmployeeId);

            return null;
        }

        public async Task<string> RegisterUserAsync(RegisterUserRequest request)
        {
            var newUserId = Guid.NewGuid().ToString();

            var rowsEffected = await _userRepository.CreateAsync(new Entities.User
            {
                Id = newUserId,
                CreatedBy = newUserId,
                CreateOn = DateTime.UtcNow,
                IsActive = true,
                Name = request.Name,
                Password = request.Password,
                Email = request.Email,
                Username = request.Username
            });

            if (rowsEffected != 1)
                throw new InsertFailedException($"Error saving userId");

            return newUserId;
        }



    }
}

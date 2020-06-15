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
        private readonly IConfiguration _configuration;
        private readonly IEmployeeRepository _employeeRepository;

        public UserService(IUserRepository userRepository, IConfiguration configuration, IEmployeeRepository employeeRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public (string userId, string employeeId, bool isAdmin)? Authenticate(LoginRequest request)
        {
            var user = _userRepository.GetUserByUsername(request.Username);

            if (user == null)
                throw new ValidationException("Authentication Failed");

            if (user.Password != CryptoHelper.Hash(user.Id + request.Username, _configuration["SecurityConfiguraiton:HashKey"]))
                throw new ValidationException("Authentication Failed");

            var employee = _employeeRepository.GetById(user.EmployeeId);

            return (user.Id, user.EmployeeId, employee.IsAdmin);
        }

        public async Task<string> RegisterUserAsync(RegisterUserRequest request, string employeeId)
        {
            var existingUser = _userRepository.GetUserByUsername(request.Username);

            if(existingUser != null)
                throw new InsertFailedException("Username already existing in the System!");

            existingUser = _userRepository.GetUserByEmployeeId(employeeId);

            if (existingUser != null)
                throw new InsertFailedException("User already registered in the System!");

            var newUserId = Guid.NewGuid().ToString();
            var password = CryptoHelper.Hash(newUserId + request.Username, _configuration["SecurityConfiguraiton:HashKey"]);

            var rowsEffected = await _userRepository.CreateAsync(new Entities.User
            {
                Id = newUserId,
                CreatedBy = newUserId,
                CreateOn = DateTime.UtcNow,
                IsActive = true,
                Name = request.Name,
                Password = password,
                Email = request.Email,
                Username = request.Username,
                EmployeeId = employeeId
            });

            if (rowsEffected != 1)
                throw new InsertFailedException($"Error saving userId");

            return newUserId;
        }



    }
}

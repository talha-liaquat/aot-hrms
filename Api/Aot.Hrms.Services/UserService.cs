using Aot.Hrms.Contracts;
using Aot.Hrms.Contracts.Repositories;
using Aot.Hrms.Contracts.Services;
using Aot.Hrms.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Aot.Hrms.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public (string token, string userId)? Authenticate(LoginRequest request)
        {
            var user = _userRepository.GetUser(request.Username, request.Password);

            if (user != null)
                return (GenerateToken(new List<Claim> { new Claim("user-id", user.Id) }), user.Id);

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

        private byte[] SecurityKey { get { return Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); } }

        private string GenerateToken(List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SecurityKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aot.Hrms.Contracts.Repositories
{
    public interface IUserRepository
    {
        public Task<int> CreateAsync(Entities.User user);
        public Entities.User GetUser(string username, string password);
        public Entities.User GetUserByUsername(string username);
    }
}

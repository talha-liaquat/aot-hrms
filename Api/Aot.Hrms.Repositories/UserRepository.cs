using Aot.Hrms.Contracts.Repositories;
using Aot.Hrms.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aot.Hrms.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<int> CreateAsync(Entities.User user)
        {
            using var context = new AotDBContext();
            await context.User.AddAsync(user);
            return await context.SaveChangesAsync();
        }

        public Entities.User GetUser(string username, string password)
        {
            using var context = new AotDBContext();
            return context.User.SingleOrDefault(x => x.Username.ToLower() == username.ToLower() && x.Password == password);
        }

        public User GetUserByUsername(string username)
        {
            using var context = new AotDBContext();
            return context.User.SingleOrDefault(x => x.Username.ToLower() == username.ToLower());
        }
    }
}

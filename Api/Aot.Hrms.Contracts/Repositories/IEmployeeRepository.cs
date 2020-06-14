using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aot.Hrms.Contracts.Repositories
{
    public interface IEmployeeRepository
    {
        public Task<int> CreateAsync(Entities.Employee employee);
        public Task<int> UpdateAsync(Entities.Employee employee);
        public Entities.Employee GetById(string id);
        public Entities.Employee GetByEmail(string email);
    }
}

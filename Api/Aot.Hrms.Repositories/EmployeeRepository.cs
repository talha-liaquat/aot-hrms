using Aot.Hrms.Contracts.Repositories;
using Aot.Hrms.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Aot.Hrms.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public async Task<int> CreateAsync(Employee employee)
        {
            using var context = new AotDBContext();
            await context.Employee.AddAsync(employee);
            return await context.SaveChangesAsync();
        }

        public Employee GetById(string id)
        {
            using var context = new AotDBContext();
            return context.Employee.SingleOrDefault(x => x.Id == id && x.IsActive);
        }

        public async Task<int> UpdateAsync(Employee employee)
        {
            using var context = new AotDBContext();
            context.Employee.Update(employee);
            return await context.SaveChangesAsync();
        }
    }
}

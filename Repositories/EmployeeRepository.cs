using Microsoft.EntityFrameworkCore;
using MvdBackend.Data;
using MvdBackend.Models;

namespace MvdBackend.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Employee> GetWithRequestsAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.AcceptedRequests)
                .Include(e => e.AssignedRequests)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByLastNameAsync(string lastName)
        {
            return await _context.Employees
                .Where(e => e.LastName.Contains(lastName))
                .ToListAsync();
        }
    }
}
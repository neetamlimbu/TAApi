using Microsoft.EntityFrameworkCore;
using TAApi.Models;

namespace TAApi.Data
{
    public class TADBContext : DbContext
    {
        public TADBContext(DbContextOptions<TADBContext> opt): base(opt)
        {

        }
        
        public DbSet<Employee> Employees { get; set; }

    }
}
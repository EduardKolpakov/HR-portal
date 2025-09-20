using HRPortalApi.Model;
using Microsoft.EntityFrameworkCore;

namespace HRPortalApi.DBModel
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Student> Student { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Vacancy> Vacancy { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
    }
}

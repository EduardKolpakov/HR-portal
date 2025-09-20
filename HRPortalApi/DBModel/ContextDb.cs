using HRPortalApi.Model;
using Microsoft.EntityFrameworkCore;

namespace HRPortalApi.DBModel
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<VacancyResponses> VacancyResponses { get; set; }
    }
}

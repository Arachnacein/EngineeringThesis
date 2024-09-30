using Domain.Entites;
using Domain.Entites.Person;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Duty> Duties { get; set; }
        public DbSet<PersonalRequests> PersonalRequests { get; set; }
        public DbSet<Swap> Swaps { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Vacation> Vacation { get; set; }

    }
}

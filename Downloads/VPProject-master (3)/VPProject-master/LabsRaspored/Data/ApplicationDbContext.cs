using Microsoft.EntityFrameworkCore;
using LabsRaspored.Models;

namespace LabsRaspored.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Semestar> Semesters { get; set; }

        public DbSet<Predmeti> Subjects { get; set; }

        public DbSet<Laboratorija> Labs { get; set; }

        public DbSet<Slot> Slots { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        }
    }
}
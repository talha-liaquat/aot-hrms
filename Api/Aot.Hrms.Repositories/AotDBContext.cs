using Microsoft.EntityFrameworkCore;
using System;

namespace Aot.Hrms.Repositories
{
    public class AotDBContext : DbContext
    {
        public DbSet<Entities.EmployeeSkill> EmployeeSkills { get; set; }
        public DbSet<Entities.Employee> Employee { get; set; }
        public DbSet<Entities.Skill> Skill { get; set; }
        public DbSet<Entities.User> User { get; set; }
        private string ConnectionString { get; set; }
        public AotDBContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public AotDBContext()
        {
            ConnectionString = "server=127.0.0.1;database=testapp;user=root;password=helloworld;port=3308";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.IsEmailVerified).IsRequired();
                entity.Property(e => e.IsAdmin).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedBy).IsRequired();
                entity.Property(e => e.CreateOn).HasDefaultValue(DateTime.UtcNow);                
            });

            modelBuilder.Entity<Entities.Skill>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedBy).IsRequired();
                entity.Property(e => e.CreateOn).HasDefaultValue(DateTime.UtcNow);
            });

            modelBuilder.Entity<Entities.EmployeeSkill>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeSkills).HasForeignKey(x => x.EmployeeId).IsRequired(true);
                entity.HasOne(d => d.Skill).WithMany(p => p.EmployeeSkills).HasForeignKey(x => x.SkillId).IsRequired(true);
                entity.Property(e => e.CreatedBy).IsRequired();
                entity.Property(e => e.CreateOn).HasDefaultValue(DateTime.UtcNow);
            });

            modelBuilder.Entity<Entities.User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreateOn).HasDefaultValue(DateTime.UtcNow);
            });
        }
    }
}
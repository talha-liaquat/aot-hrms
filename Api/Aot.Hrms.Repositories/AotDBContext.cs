using Aot.Hrms.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

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
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.EmployeeId).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreateOn).HasDefaultValue(DateTime.UtcNow);
            });
        }

        public static void Initialize(string hashKey)
        {
            using (var context = new AotDBContext())
            {
                context.Database.EnsureCreated();

                if (!context.User.Any(x => x.Username == "admin"))
                {
                    var employeeId = Guid.NewGuid().ToString();
                    context.Employee.Add(new Entities.Employee
                    {
                        Id = employeeId,
                        CreatedBy = employeeId,
                        CreateOn = DateTime.Now,
                        Email = "talha.liaquat@gmail.com",
                        IsActive = true,
                        IsAdmin = true,
                        Name = "admin",
                        IsEmailVerified = true
                    });

                    var userId = Guid.NewGuid().ToString();
                    var username = "admin";
                    var password = CryptoHelper.Hash(userId + username, hashKey);

                    context.User.Add(new Entities.User
                    {
                        Id = userId,
                        CreatedBy = employeeId,
                        CreateOn = DateTime.Now,
                        EmployeeId = employeeId,
                        IsActive = true,
                        Name = username,
                        Username = username,
                        Password = password,
                        Email = "talha.liaquat@gmail.com"
                    });
                }
                context.SaveChanges();
            }
        }
    }
}
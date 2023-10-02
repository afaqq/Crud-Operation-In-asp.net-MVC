using Microsoft.EntityFrameworkCore;
using serverside.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using serverside.Controllers;

namespace serverside.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public virtual DbSet<Academic> Academics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Academic)
                .WithOne(a => a.Student)
                .HasForeignKey(a => a.StudentId);
        }

      
    }
}

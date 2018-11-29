using Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
namespace infrastructure
{
    public class BazarContext : DbContext
    {
        public BazarContext(DbContextOptions<BazarContext> options) : base(options)
        {
            
        }

        //public DbSet<> {get; set;}
        //public DbSet<> {get; set;}
        //public DbSet<> {get; set;}

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        }
    }
}

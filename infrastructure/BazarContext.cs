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

        public DbSet<Booth> Booth {get; set;}
        public DbSet<User> User {get; set;}
        //public DbSet<> {get; set;}

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booth>().HasOne(b => b.Booker).WithOne().HasForeignKey<User>(u => u.Id);
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        }
    }
}

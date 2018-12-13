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
        public DbSet<WaitingListItem> WaitingListItem { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ImageURL> ImageURL { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booth>().HasOne<User>(b => b.Booker).WithMany().OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<WaitingListItem>().HasOne<User>(w => w.Booker).WithMany().OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
        }
    }
}

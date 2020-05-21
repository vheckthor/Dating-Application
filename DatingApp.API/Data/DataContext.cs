using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext>options):base(options)
        {
            
        }
        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<PhoneNumberDetails> PhoneNumber { get; set; }
        public DbSet<Like> Likes { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Like>()
            .HasKey(k => new {k.LikerId, k.LikeeId});

            builder.Entity<Like>()
            .HasOne(u => u.Likee)
            .WithMany(u => u.Liker)
            .HasForeignKey(u => u.LikeeId)
            //.HasPrincipalKey(u => u.UserUniqueIdentity)
            .OnDelete(DeleteBehavior.Restrict);

            
            builder.Entity<Like>()
            .HasOne(u => u.Liker)
            .WithMany(u => u.Likee)
            .HasForeignKey(u => u.LikerId)
            //.HasPrincipalKey(u => u.UserUniqueIdentity)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }


}
using Microsoft.EntityFrameworkCore;
using API.Entities;


namespace API.Data
{

    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           
            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserId });
            builder.Entity<UserLike>()
                .HasOne(u => u.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(f => f.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<UserLike>()
                .HasOne(u => u.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(f => f.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
            
        }
        

    }

}
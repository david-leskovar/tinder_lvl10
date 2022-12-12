using Tinder_lvl10.Entities;
using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace Tinder_lvl10.Data
{
    public class DataContext:DbContext
    {

        private readonly IConfiguration _config;

        public DataContext(DbContextOptions options,IConfiguration config) : base(options) {

            this._config = config;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_config.GetConnectionString("DefaultConnection"));
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }

        public DbSet<Message> Messages  { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {

            base.OnModelCreating(builder);
            builder.Entity<UserLike>().HasKey(k => new { k.SourceUserId, k.LikedUserId });

            builder.Entity<UserLike>().HasOne(s => s.SourceUser).WithMany(l => l.LikedUsers).HasForeignKey(s => s.SourceUserId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>().HasOne(s => s.LikedUser).WithMany(l => l.LikedByUsers).HasForeignKey(s => s.LikedUserId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>().HasOne(u => u.Recipient).WithMany(m=>m.MessagesReceived).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>().HasOne(u => u.Sender).WithMany(m => m.MessagesSent).OnDelete(DeleteBehavior.Restrict);

        }


    }
}

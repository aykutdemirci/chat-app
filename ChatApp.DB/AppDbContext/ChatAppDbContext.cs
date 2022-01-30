using ChatApp.Dto;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Db.AppDbContext
{
    public class ChatAppDbContext : DbContext
    {
        public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatRoom>().HasMany(r => r.Messages).WithOne(m => m.Room).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChatRoom>().HasData(new ChatRoom { Id = 1, Name = "Default" }, new ChatRoom { Id = 2, Name = "Oda-1" });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ChatRoom> ChatRooms { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}

using System.IO;
using Microsoft.EntityFrameworkCore;
using tpiPracticeClasses;

namespace tpiPracticeASPNET
{
    public class TpiDB : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Groupchat> Groupchats => Set<Groupchat>();

        public TpiDB(DbContextOptions<TpiDB> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

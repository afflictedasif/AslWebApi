using AslWebApi.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AslWebApi.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> opts) : base(opts) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=(local);Database=SchoolDB;Trusted_Connection=True");
        //}

        //For logging.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Unique key in UserInfos, act as primary key
            modelBuilder.Entity<UserInfo>()
                .HasIndex(u => new { u.UserID })
                .IsUnique(true);

            //Unique key in UserInfos
            modelBuilder.Entity<UserInfo>()
                .HasIndex(u => new { u.EmailID })
                .IsUnique(true);
            modelBuilder.Entity<UserInfo>()
                .HasIndex(u => new { u.LoginID, })
                .IsUnique(true);
            modelBuilder.Entity<UserInfo>()
                .HasIndex(u => new { u.MobNo })
                .IsUnique(true);

            //Unique key in UserState
            modelBuilder.Entity<UserState>()
                .HasIndex(u => new { u.UserID })
                .IsUnique(true);

        }

        public DbSet<UserInfo> UserInfos => Set<UserInfo>();
        public DbSet<UserState> UserStates => Set<UserState>();
        public DbSet<CLog> CLogs => Set<CLog>();
        public DbSet<ScreenShot> ScreenShots => Set<ScreenShot>();


    }
}

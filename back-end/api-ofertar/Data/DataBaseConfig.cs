using Microsoft.EntityFrameworkCore;
using api_ofertar.Entities;

namespace api_ofertar.Data
{
    public class DataBaseConfig : DbContext
    {
        public DataBaseConfig(DbContextOptions<DataBaseConfig> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Tither> Tithers { get; set; }
        public DbSet<Tithe> Tithes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Email, u.Name })
                .IsUnique();

            // Tither self-reference
            modelBuilder.Entity<Tither>()
                .HasOne(t => t.Spouse)
                .WithOne()
                .HasForeignKey<Tither>(t => t.SpouseId)
                .OnDelete(DeleteBehavior.SetNull);

            // Tither -> Profession
            modelBuilder.Entity<Tither>()
                .HasOne(t => t.Profession)
                .WithMany(p => p.Tithers)
                .HasForeignKey(t => t.ProfessionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Tither -> Address
            modelBuilder.Entity<Tither>()
                .HasOne(t => t.Address)
                .WithMany()
                .HasForeignKey(t => t.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tithe -> Tither
            modelBuilder.Entity<Tithe>()
                .HasOne(t => t.Tither)
                .WithMany(ti => ti.Tithes)
                .HasForeignKey(t => t.TitherId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tithe -> User
            modelBuilder.Entity<Tithe>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tithes)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }    
}
using Microsoft.EntityFrameworkCore;
using api_ofertar.Entities;
using api_ofertar.Enums;

namespace api_ofertar.Data
{
    public class DataBaseConfig : DbContext
    {
        public DataBaseConfig(DbContextOptions<DataBaseConfig> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Church> Churches { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Tither> Tithers { get; set; }
        public DbSet<Tithe> Tithes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relations

            // User
            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Email, u.Name })
                .IsUnique();

            // User -> Church
            modelBuilder.Entity<User>()
                .HasOne(u => u.Church)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.ChurchId)
                .OnDelete(DeleteBehavior.SetNull);

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

            // Tither -> Church
            modelBuilder.Entity<Tither>()
                .HasOne(t => t.Church)
                .WithMany()
                .HasForeignKey(t => t.ChurchId)
                .OnDelete(DeleteBehavior.SetNull);

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

            // UserRole -> User
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // UserRole -> Role
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Enum settings

            // Enum Marital Status
            modelBuilder.Entity<Tither>()
                .Property(t => t.MaritalStatus)
                .HasConversion(
                    v => (char)v,
                    v => (MaritalStatus)v
                )
                .HasColumnType("char(1)");
            
            // Enum Payment Method
            modelBuilder.Entity<Tithe>()
                .Property(t => t.PaymentMethod)
                .HasConversion(
                    v => (char)v,
                    v => (char)(PaymentMethod)v
                )
                .HasColumnType("char(1)");
        }
    }    
}
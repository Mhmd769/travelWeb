using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using user_adminlogin.Models;

namespace user_adminlogin.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Package> Packages { get; set; }

        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // Configure IdentityUserLogin
            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
            });
            modelBuilder.Entity<Package>()
                .HasOne(p => p.Flight)
                .WithMany(f => f.Packages)
                .HasForeignKey(p => p.FlightId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Flight)
                .WithMany(f => f.Feedbacks)
                .HasForeignKey(f => f.FlightId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UserId);

            // Many-to-many relationship between ApplicationUser and Flight using UserFlight join entity
            modelBuilder.Entity<Booking>()
                .HasKey(uf => new { uf.UserId, uf.FlightId });

            modelBuilder.Entity<Booking>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.UserFlights)
                .HasForeignKey(uf => uf.UserId);

            modelBuilder.Entity<Booking>()
                .HasOne(uf => uf.Flight)
                .WithMany(f => f.UserFlights)
                .HasForeignKey(uf => uf.FlightId);
        }



    }
}
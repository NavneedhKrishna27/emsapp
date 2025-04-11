using EventManagementSystemMerged.Models;
using Microsoft.EntityFrameworkCore;
using System;


namespace EventManagementSystemMerged.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=EventManagementSystemMerged1;Integrated Security=True;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(e => e.CategoryID);

            modelBuilder.Entity<Event>()
                .HasOne<Location>()
                .WithMany()
                .HasForeignKey(e => e.LocationID);


            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(l => l.LocationID);
                entity.Property(l => l.LocationName).IsRequired().HasMaxLength(255);
                entity.Property(l => l.PrimaryContact).IsRequired().HasMaxLength(15);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserID);
                entity.Property(u => u.Name).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Password).IsRequired().HasMaxLength(255);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.TicketID);
                entity.Property(t => t.BookingDate).IsRequired();
                entity.Property(t => t.Status).IsRequired().HasMaxLength(50);

                entity.HasOne<Event>()
                      .WithMany(e => e.Tickets)
                      .HasForeignKey(t => t.EventID)
                      .OnDelete(DeleteBehavior.NoAction); 

                entity.HasOne<User>()
                      .WithMany(u => u.Tickets)
                      .HasForeignKey(t => t.UserID)
                      .OnDelete(DeleteBehavior.NoAction); 
            });
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.NotificationID);
                entity.Property(n => n.Message).IsRequired();
                entity.Property(n => n.SentTimestamp).IsRequired();
                entity.HasOne<User>()
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserID)
                      .OnDelete(DeleteBehavior.NoAction); 

                entity.HasOne<Event>()
                      .WithMany(e => e.Notifications)
                      .HasForeignKey(n => n.EventID)
                      .OnDelete(DeleteBehavior.NoAction); 
            });
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(f => f.FeedbackID);
                entity.Property(f => f.Rating).IsRequired();
                entity.Property(f => f.SubmittedTimestamp).IsRequired();
                entity.HasOne<Event>()
                      .WithMany(e => e.Feedbacks)
                      .HasForeignKey(f => f.EventID)
                      .OnDelete(DeleteBehavior.Restrict); 

                entity.HasOne<User>()
                      .WithMany(u => u.Feedbacks)
                      .HasForeignKey(f => f.UserID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne<Ticket>()
.WithOne(t => t.Feedback)
.HasForeignKey<Feedback>(f => f.TicketID)
.OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentID);
                entity.Property(p => p.Amount).IsRequired().HasColumnType("decimal(10, 2)");
                entity.Property(p => p.PaymentDate).IsRequired();
                entity.Property(p => p.PaymentStatus).IsRequired().HasMaxLength(50);
                entity.HasOne<User>()
                      .WithMany(u => u.Payments)
                      .HasForeignKey(p => p.UserID)
                      .OnDelete(DeleteBehavior.NoAction); 
                entity.HasOne<Event>()
                      .WithMany(e => e.Payments)
                      .HasForeignKey(p => p.EventID)
                      .OnDelete(DeleteBehavior.NoAction); 
            });
        }
    }
}
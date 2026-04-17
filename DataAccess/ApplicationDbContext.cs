using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Models;
using Models.Models;
using System.Linq;
using System.Reflection;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<AiReport> AiReports { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
       
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ClinicPhone> ClinicPhones { get; set; }
        public DbSet<DoctorAvilableTime> DoctorAvilableTimes { get; set; }
        public DbSet<Nures> Nures { get; set; }
        public DbSet<PatientPhone> patientPhones { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<DoctorPatient> DoctorPatients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<PharmacyProduct> PharmacyProducts { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<PharmacyRating> PharmacyRatings { get; set; }

        public DbSet<Conversation> Conversations{ get; set; }
        public DbSet<ConversationParticipant> conversationParticipants { get; set; }
        public DbSet<Messages> Messages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Inventory>()
                .HasOne(i => i.Pharmacy)
                .WithMany(p => p.Inventories)
                .HasForeignKey(i => i.PharmacyId)
                .HasPrincipalKey(p => p.ID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Inventory>()
                .HasOne(i => i.PharmacyProduct)
                .WithMany(pp => pp.Inventories)
                .HasForeignKey(i => i.PharmacyProductId)
                .HasPrincipalKey(pp => pp.ID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .HasPrincipalKey(o => o.ID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Inventory)
                .WithMany()
                .HasForeignKey(oi => oi.InventoryId)
                .HasPrincipalKey(i => i.ID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Invoice>()
                .HasOne(inv => inv.Order)
                .WithOne(o => o.Invoice)
                .HasForeignKey<Invoice>(inv => inv.OrderId)
                .HasPrincipalKey<Order>(o => o.ID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.Patient)
                .WithMany()
                .HasForeignKey(o => o.PatientId)
                .HasPrincipalKey(p => p.ID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.Pharmacy)
                .WithMany()
                .HasForeignKey(o => o.PharmacyId)
                .HasPrincipalKey(p => p.ID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Inventory>()
                .Property(i => i.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Invoice>()
                .Property(inv => inv.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var relationship in builder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor>Doctors  { get; set; }
        public DbSet<AiReport> AiReports { get; set; }
        public DbSet<Appointment>Appointments  { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ClinicPhone> ClinicPhones { get; set; }
        public DbSet<DoctorAvilableTime> doctorAvilableTimes { get; set; }
        public DbSet<Nures> Nures { get; set; }
        public DbSet<PatientPhone> patientPhones { get; set; }
        public DbSet<Presciption> presciptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<Rating>  Ratings { get; set; }
        public DbSet<Specialization>  Specializations { get; set; }
        public DbSet<DoctorPatient>  DoctorPatients { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)   
        {
            
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Specialization>()
          .HasMany(e => e.Doctor)
           .WithOne(d => d.Specialization)
           .HasForeignKey(d => d.SpecializationId)
             .OnDelete(DeleteBehavior.SetNull);



            builder.Entity<DoctorPatient>()
                .HasOne(e => e.Patient)
                .WithMany(s => s.DoctorPatients)
                .HasForeignKey(sc => sc.PatientId)
                 .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<DoctorPatient>()
                .HasOne(e => e.Doctor)
                .WithMany(s => s.DoctorPatients)
                .HasForeignKey(sc => sc.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.Entity<Doctor>()
                .HasMany(d => d.Clinics)
                .WithOne(c => c.Doctor)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.Cascade);

            // Doctor → Appointments (Cascade)
            builder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Doctor → DoctorAvailableTime (Cascade)
            builder.Entity<DoctorAvilableTime>()
                .HasOne(t => t.Doctor)
                .WithMany(d => d.avilableTimes)
                .HasForeignKey(t => t.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Doctor → Prescriptions (Cascade)
            builder.Entity<Presciption>()
                .HasOne(p => p.Doctor)
                .WithMany(d => d.Presciptions)
                .HasForeignKey(p => p.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            // SetNull for ChatMessage → User
            builder.Entity<ChatMessage>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);
        
            builder.Entity<ChatMessage>().HasOne(s => s.User).WithMany(s => s.Messages).HasForeignKey(s => s.ApplicationUserId).OnDelete(DeleteBehavior.SetNull);

            
            builder.Entity<Doctor>().HasIndex(e => new { e.RattingAverage, e.City, e.FullName });
            builder.Entity<Specialization>().HasIndex(e => new { e.Name});
            builder.Entity<Nures>().HasIndex(e => new { e.FullName, e.Address, e.City, e.Experence,e.RattingAverage });
        }

    }
}

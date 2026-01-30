using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Reflection;
using System.Reflection.Emit;

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
        public DbSet<RefreshToken>  RefreshTokens { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)   
        {
            
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}

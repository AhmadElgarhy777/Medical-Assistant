using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace DataAccess.Config
{
    internal class PatientMedicalScanConfiguration : IEntityTypeConfiguration<PatientMedicalScan>
    {
        public void Configure(EntityTypeBuilder<PatientMedicalScan> builder)
        {
            builder.Property(x => x.ImagePath)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.DoctorNote)
                .HasMaxLength(1000);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.HasOne(x => x.Patient)
                .WithMany()
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Doctor)
                .WithMany()
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AiReport)
                .WithOne()
                .HasForeignKey<PatientMedicalScan>(x => x.AiReportId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

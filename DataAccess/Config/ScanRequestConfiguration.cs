using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Config
{
    internal class ScanRequestConfiguration : IEntityTypeConfiguration<ScanRequest>
    {
        public void Configure(EntityTypeBuilder<ScanRequest> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.AIModelType)
                   .HasConversion<string>();

            builder.Property(x => x.Status)
                   .HasConversion<string>();

            builder.Property(x => x.DoctorNote)
                   .HasMaxLength(1000);

            builder.HasOne(x => x.Doctor)
             .WithMany()
             .HasForeignKey(x => x.DoctorId)
             .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Patient)
                   .WithMany()
                   .HasForeignKey(x => x.PatientId)
                   .OnDelete(DeleteBehavior.NoAction);

            // ScanRequest -> Images
            builder.HasMany(x => x.Images)
                   .WithOne(x => x.ScanRequest)
                   .HasForeignKey(x => x.ScanRequestId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ScanRequest -> AiReport (One To One)
            builder.HasOne(x => x.AiReport)
                   .WithOne(x => x.ScanRequest)
                   .HasForeignKey<AiReport>(x => x.ScanRequestId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

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
    internal class AiReportConfigration : IEntityTypeConfiguration<AiReport>
    {
        public void Configure(EntityTypeBuilder<AiReport> builder)
        {
            builder.Property(x => x.Diagnosis)
                 .HasMaxLength(200)
                 .IsRequired();

            builder.Property(x => x.ModelType)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Confidence)
                .HasPrecision(5, 2);

            builder.Property(x => x.RawApiResponse)
                .HasColumnType("nvarchar(max)");

            builder.HasOne(x => x.Patient)
                .WithMany(x => x.AiReports)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Doctor)
                .WithMany(x => x.AiReports)
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Images)
                .WithOne(x => x.AiReport)
                .HasForeignKey(x => x.AiReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

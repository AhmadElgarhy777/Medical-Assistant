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
            builder
                .HasOne(m => m.Patient)
                .WithMany(u => u.AiReports)
                .HasForeignKey(f => f.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(m => m.Doctor)
                .WithMany(u => u.Reports)
                .HasForeignKey(f => f.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

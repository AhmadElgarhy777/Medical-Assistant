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
    internal class DoctorPatientConfigration : IEntityTypeConfiguration<DoctorPatient>
    {
        public void Configure(EntityTypeBuilder<DoctorPatient> builder)
        {

            builder
                .HasOne(e => e.Patient)
                .WithMany(s => s.DoctorPatients)
                .HasForeignKey(sc => sc.PatientId)
                 .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(e => e.Doctor)
                .WithMany(s => s.DoctorPatients)
                .HasForeignKey(sc => sc.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

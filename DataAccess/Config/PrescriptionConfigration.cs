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
    internal class PrescriptionConfigration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder
               .HasOne(p => p.Doctor)
               .WithMany(d => d.Prescriptions)
               .HasForeignKey(p => p.DoctorId)
               .OnDelete(DeleteBehavior.Cascade);


            builder
                .HasOne(p => p.Patient)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

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
    internal class PresciptionConfigration : IEntityTypeConfiguration<Presciption>
    {
        public void Configure(EntityTypeBuilder<Presciption> builder)
        {
            builder
               .HasOne(p => p.Doctor)
               .WithMany(d => d.Presciptions)
               .HasForeignKey(p => p.DoctorId)
               .OnDelete(DeleteBehavior.Cascade);


            builder
                .HasOne(p => p.Patient)
                .WithMany(d => d.presciptions)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

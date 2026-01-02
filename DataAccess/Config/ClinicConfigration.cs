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
    internal class ClinicConfigration : IEntityTypeConfiguration<Clinic>
    {
        public void Configure(EntityTypeBuilder<Clinic> builder)
        {
                    builder
                    .HasOne(c => c.Doctor)
                    .WithMany(d => d.Clinics)
                    .HasForeignKey(c => c.DoctorId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

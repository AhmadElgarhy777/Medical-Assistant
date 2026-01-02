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
    internal class DoctorAvilableTimeConfigration : IEntityTypeConfiguration<DoctorAvilableTime>
    {
        public void Configure(EntityTypeBuilder<DoctorAvilableTime> builder)
        {
            builder
               .HasOne(t => t.Doctor)
               .WithMany(d => d.avilableTimes)
               .HasForeignKey(t => t.DoctorId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

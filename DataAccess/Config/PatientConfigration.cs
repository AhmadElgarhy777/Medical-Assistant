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
    internal class PatientConfigration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.Property(e => e.ID).IsRequired();
            builder.Property(e => e.SSN).IsRequired();
            builder.Property(e => e.FullName).IsRequired();
            builder.Property(e => e.Email).IsRequired();
            builder.Property(e => e.Gender).IsRequired();
            builder.Property(e => e.BD).IsRequired();
            builder.Property(e => e.Governorate).IsRequired();
            builder.Property(e => e.Address).IsRequired();
            builder.Property(e => e.City).IsRequired();
        }
    }
}

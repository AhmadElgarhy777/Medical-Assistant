using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace DataAccess.Config
{
    internal class MedicalTestConfiguration : IEntityTypeConfiguration<MedicalTest>
    {
        public void Configure(EntityTypeBuilder<MedicalTest> builder)
        {
            builder.Property(t => t.BasePrice).HasColumnType("decimal(10,2)");
        }
    }
}
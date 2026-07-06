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
    internal class LabConfiguration : IEntityTypeConfiguration<Lab>
    {
        public void Configure(EntityTypeBuilder<Lab> builder)
        {
            builder
                .HasOne(l => l.Area)
                .WithMany()
                .HasForeignKey(l => l.AreaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

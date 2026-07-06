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
    internal class RadiologyCenterScanConfiguration : IEntityTypeConfiguration<RadiologyCenterScan>
    {
        public void Configure(EntityTypeBuilder<RadiologyCenterScan> builder)
        {
            builder
                .HasOne(cs => cs.RadiologyCenter)
                .WithMany(c => c.ScanOffers)
                .HasForeignKey(cs => cs.RadiologyCenterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(cs => cs.RadiologyScan)
                .WithMany(s => s.CenterOffers)
                .HasForeignKey(cs => cs.RadiologyScanId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(cs => cs.Price).HasColumnType("decimal(10,2)");

            builder.HasIndex(cs => new { cs.RadiologyCenterId, cs.RadiologyScanId }).IsUnique();
        }
    }
}

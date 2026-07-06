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
    internal class LabBookingConfiguration : IEntityTypeConfiguration<LabBooking>
    {
        public void Configure(EntityTypeBuilder<LabBooking> builder)
        {
            builder
                .HasOne(b => b.Lab)
                .WithMany()
                .HasForeignKey(b => b.LabId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.RadiologyCenter)
                .WithMany()
                .HasForeignKey(b => b.RadiologyCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.Area)
                .WithMany()
                .HasForeignKey(b => b.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(b => b.Items)
                .WithOne(i => i.LabBooking)
                .HasForeignKey(i => i.LabBookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(b => b.SubTotal).HasColumnType("decimal(10,2)");
            builder.Property(b => b.HomeCollectionFee).HasColumnType("decimal(10,2)");
            builder.Property(b => b.DiscountAmount).HasColumnType("decimal(10,2)");
            builder.Property(b => b.TotalPrice).HasColumnType("decimal(10,2)");
        }
    }
}

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
    internal class LabBookingItemConfiguration : IEntityTypeConfiguration<LabBookingItem>
    {
        public void Configure(EntityTypeBuilder<LabBookingItem> builder)
        {
            builder
                .HasOne(i => i.MedicalTest)
                .WithMany()
                .HasForeignKey(i => i.MedicalTestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(i => i.RadiologyScan)
                .WithMany()
                .HasForeignKey(i => i.RadiologyScanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(i => i.Result)
                .WithOne(r => r.LabBookingItem)
                .HasForeignKey<LabTestResult>(r => r.LabBookingItemId);

            builder.Property(i => i.Price).HasColumnType("decimal(10,2)");
        }
    }
}
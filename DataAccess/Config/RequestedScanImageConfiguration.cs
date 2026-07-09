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
    internal class RequestedScanImageConfiguration
     : IEntityTypeConfiguration<RequestedScanImage>
    {
        public void Configure(EntityTypeBuilder<RequestedScanImage> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ImagePath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne(x => x.ScanRequest)
                   .WithMany(x => x.Images)
                   .HasForeignKey(x => x.ScanRequestId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

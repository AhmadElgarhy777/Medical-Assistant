using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiService.Configuration
{
    public class AiReportImageConfiguration
     : IEntityTypeConfiguration<AiReportImage>
    {
        public void Configure(EntityTypeBuilder<AiReportImage> builder)
        {
           
            builder.Property(x => x.ImagePath)
                .HasMaxLength(500);

            builder.Property(x => x.ContentType)
                .HasMaxLength(100);
        }
    }
}

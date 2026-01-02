using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Config
{
    internal class NuresConfigartion : IEntityTypeConfiguration<Nures>
    {
        public void Configure(EntityTypeBuilder<Nures> builder)
        {
            builder.HasIndex(e => new { e.FullName, e.Address, e.City, e.Experence, e.RattingAverage });
        
            builder.Property(e => e.ID).IsRequired();
            builder.Property(e => e.SSN).IsRequired();
            builder.Property(e => e.FullName).IsRequired();
            builder.Property(e => e.Email).IsRequired();
            builder.Property(e => e.Gender).IsRequired();
            builder.Property(e => e.Img).IsRequired();
            builder.Property(e => e.BD).IsRequired();
            builder.Property(e => e.Phone).IsRequired();
            builder.Property(e => e.Governorate).IsRequired();
            builder.Property(e => e.Address).IsRequired();
            builder.Property(e => e.City).IsRequired();
            builder.Property(e => e.CrediateImg).IsRequired();
            builder.Property(e => e.Degree).IsRequired();
            builder.Property(e => e.Experence).IsRequired();
            builder.Property(e => e.PricePerDay).IsRequired();
        }
    }
}

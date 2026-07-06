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
    internal class RadiologyCenterConfiguration : IEntityTypeConfiguration<RadiologyCenter>
    {
        public void Configure(EntityTypeBuilder<RadiologyCenter> builder)
        {
            builder
                .HasOne(c => c.Area)
                .WithMany()
                .HasForeignKey(c => c.AreaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
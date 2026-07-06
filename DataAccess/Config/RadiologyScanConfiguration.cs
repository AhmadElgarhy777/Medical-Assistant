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
    internal class RadiologyScanConfiguration : IEntityTypeConfiguration<RadiologyScan>
    {
        public void Configure(EntityTypeBuilder<RadiologyScan> builder)
        {
            builder.Property(s => s.BasePrice).HasColumnType("decimal(10,2)");
        }
    }
}
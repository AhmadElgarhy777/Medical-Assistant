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
    internal class PrescriptionItemConfigartion : IEntityTypeConfiguration<PrescriptionItem>

    {
        public void Configure(EntityTypeBuilder<PrescriptionItem> builder)
        {
            builder
                .HasOne(p => p.Presciption)
                .WithMany(d => d.items)
                .HasForeignKey(p => p.PresciptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

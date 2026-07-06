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
    internal class LabTestOfferConfiguration : IEntityTypeConfiguration<LabTestOffer>
    {
        public void Configure(EntityTypeBuilder<LabTestOffer> builder)
        {
            builder
                .HasOne(lt => lt.Lab)
                .WithMany(l => l.TestOffers)
                .HasForeignKey(lt => lt.LabId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(lt => lt.MedicalTest)
                .WithMany(t => t.LabOffers)
                .HasForeignKey(lt => lt.MedicalTestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(lt => lt.Price).HasColumnType("decimal(10,2)");

            builder.HasIndex(lt => new { lt.LabId, lt.MedicalTestId }).IsUnique();
        }
    }
}

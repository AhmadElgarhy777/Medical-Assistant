using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Config
{
    internal class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.Title)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.Body)
                   .HasMaxLength(1000)
                   .IsRequired();

            builder.Property(x => x.Type)
                   .HasConversion<string>();

            builder.Property(x => x.ReferenceType)
                   .HasConversion<string>();

            builder.HasOne(x => x.Sender)
                   .WithMany(x => x.SentNotifications)
                   .HasForeignKey(x => x.SenderId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Receiver)
                   .WithMany(x => x.ReceivedNotifications)
                   .HasForeignKey(x => x.ReceiverId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

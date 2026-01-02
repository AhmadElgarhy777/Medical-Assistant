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
    internal class ChatMessageConfigartion : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder
               .HasOne(m => m.User)
               .WithMany(u => u.Messages)
               .HasForeignKey(m => m.ApplicationUserId)
               .OnDelete(DeleteBehavior.SetNull);
         
            builder.HasOne(s => s.User).WithMany(s => s.Messages).HasForeignKey(s => s.ApplicationUserId).OnDelete(DeleteBehavior.SetNull);


        }
    }
}

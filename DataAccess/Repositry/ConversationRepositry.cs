using DataAccess.Repositry.IRepositry;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class ConversationRepositry:Repositry<Conversation>,IConversationRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public ConversationRepositry(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

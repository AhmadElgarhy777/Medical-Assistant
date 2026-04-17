using DataAccess.Repositry.IRepositry;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class MessageRepositry:Repositry<Messages>,IMessageRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public MessageRepositry(ApplicationDbContext dbContext):base(dbContext) 
        {
            this.dbContext = dbContext;
        }
    }
}

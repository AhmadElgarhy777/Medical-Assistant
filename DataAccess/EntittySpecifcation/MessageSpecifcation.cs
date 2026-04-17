using DataAccess.Specfications;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntittySpecifcation
{
    public class MessageSpecifcation:Specfication<Messages>
    {
        public MessageSpecifcation(string convesationId):base(m=>m.ConversationId==convesationId)
        {
            
        }
        public MessageSpecifcation(string convesationId, string userId, bool IsRead)
            :base(m=>m.ConversationId==convesationId && m.SenderId==userId && m.IsRead==false)
        {
            
        }
    }
}

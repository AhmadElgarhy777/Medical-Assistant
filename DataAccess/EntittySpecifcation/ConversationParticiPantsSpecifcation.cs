using DataAccess.Repositry;
using DataAccess.Specfications;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntittySpecifcation
{
    public class ConversationParticiPantsSpecifcation:Specfication<ConversationParticipant>
    {
        public ConversationParticiPantsSpecifcation(string conversationId, string userId)
            :base(c=>c.ConversationId==conversationId && c.UserId==userId)
        {
            
        }
    }
}

using DataAccess.Repositry.IRepositry;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class NotificationRepository:Repositry<Notification>,INotificationRepository
    {
         private readonly ApplicationDbContext dbContext;

        public NotificationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

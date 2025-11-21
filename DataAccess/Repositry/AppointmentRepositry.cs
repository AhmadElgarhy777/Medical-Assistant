using DataAccess.Repositry.IRepositry;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class AppointmentRepositry:Repositry<Appointment>,IAppointmentRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public AppointmentRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

using DataAccess.Specfications;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntittySpecifcation
{
    public class AppointmentSpescifcation:Specfication<Appointment>
    {
        public AppointmentSpescifcation():base(x => x.IsDeleted == false)
        {
            Includes.Add(a => a.Patient);
            Includes.Add(a => a.Doctor);
        }
        public AppointmentSpescifcation(Expression<Func<Appointment,bool>> expression):base(expression)
        {
            Includes.Add(a => a.Patient);
            Includes.Add(a => a.Doctor);
        }
    }
}

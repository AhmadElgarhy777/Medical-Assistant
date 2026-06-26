using DataAccess.Specfications;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntittySpecifcation
{
    public class DoctorSpecifcation : Specfication<Doctor>
    {
        public DoctorSpecifcation():base(a=>a.IsDeleted == false)
        {
            Includes?.Add(p => p.Specialization);
        }
        public DoctorSpecifcation(string Id):base(d=>d.ID==Id && d.IsDeleted == false)
        {
            Includes?.Add(p => p.Specialization);
        }
      
        public DoctorSpecifcation(Expression<Func<Doctor,bool>> expression ):base(expression)
        {
            Includes?.Add(p => p.Specialization);
        }
        public DoctorSpecifcation(ConfrmationStatus Status):base(d=>d.Status.Equals(Status) && d.IsDeleted == false)
        {
            Includes?.Add(p => p.Specialization);
        }
    }
}

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
    public class PatientSpecifcation:Specfication<Patient>
    {
        public PatientSpecifcation(string Id):base(p=>p.ID==Id&&p.IsDeleted==false)
        {
            
        }
        public PatientSpecifcation(Expression<Func<Patient, bool>> expression) : base(expression)
        {
            
        }
    }
}

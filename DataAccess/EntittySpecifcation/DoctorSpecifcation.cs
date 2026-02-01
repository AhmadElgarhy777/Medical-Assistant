using DataAccess.Specfications;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntittySpecifcation
{
    public class DoctorSpecifcation : Specfication<Doctor>
    {
        public DoctorSpecifcation():base()
        {
            Includes?.Add(p => p.Specialization);
        }
        public DoctorSpecifcation(string Id):base(d=>d.ID==Id)
        {
            Includes?.Add(p => p.Specialization);
        }
    }
}

using DataAccess.Specfications;
using Models;
using Models.Enums;
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
        public DoctorSpecifcation(ConfrmationStatus Status):base(d=>d.Status.Equals(Status))
        {
            Includes?.Add(p => p.Specialization);
        }
    }
}

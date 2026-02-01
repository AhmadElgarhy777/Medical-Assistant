using DataAccess.Specfications;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntittySpecifcation
{
    public class PatientSpecifcation:Specfication<Patient>
    {
        public PatientSpecifcation(string Id):base(p=>p.ID==Id)
        {
            
        }
    }
}

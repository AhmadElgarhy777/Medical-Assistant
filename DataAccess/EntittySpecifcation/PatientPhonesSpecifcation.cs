using DataAccess.Specfications;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntittySpecifcation
{
    public class PatientPhonesSpecifcation:Specfication<PatientPhone>
    {
        public PatientPhonesSpecifcation(string Id):base(e=>e.PatientId==Id)
        {
            
        }
    }
}

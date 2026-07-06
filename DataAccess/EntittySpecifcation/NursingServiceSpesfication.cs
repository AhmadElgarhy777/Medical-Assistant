using DataAccess.Specfications;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntittySpecifcation
{
   
    public class NursingServiceSpesfication : Specfication<NursingService>
    {
        public NursingServiceSpesfication(string id)
        : base(x => x.ID == id && x.IsDeleted == false)
        {
        }
       
    }
    public class NursingServiceByNameSpesfication : Specfication<NursingService>
    {
        public NursingServiceByNameSpesfication(string name)
        : base(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == false)
        {
        }

    }
    public class NurseServiceSpesfication : Specfication<NurseService>
    {
        public NurseServiceSpesfication(string nurseId, string serviceId)
       : base(x => x.NurseId == nurseId && x.ServiceId == serviceId && x.IsDeleted == false)
        {
            Includes.Add(x => x.Service);
            Includes.Add(x => x.Nurse);
        }

        public NurseServiceSpesfication(string nurseId)
        : base(x => x.NurseId == nurseId && x.IsDeleted == false)
        {
            Includes.Add(x => x.Service);
        }
    }



}

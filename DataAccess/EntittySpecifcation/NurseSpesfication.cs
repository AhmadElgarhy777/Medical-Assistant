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
    public class NurseSpesfication:Specfication<Nures>
    {
        public NurseSpesfication():base()
        {
            Includes.Add(e => e.Ratings);
        }
        public NurseSpesfication(string Id):base(s=>s.ID==Id)
        {
        }
        public NurseSpesfication(ConfrmationStatus Status) : base(d => d.Status.Equals(Status))
        {
        }
    }
}

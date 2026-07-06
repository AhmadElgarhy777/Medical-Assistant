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
    public class NurseSpesfication:Specfication<Nures>
    {
        public NurseSpesfication():base(x => x.IsDeleted == false)
        {
        }
        public NurseSpesfication(string Id):base(s=>s.ID==Id&&s.IsDeleted==false)
        {
            Includes.Add(e => e.NurseServices);
        }
        public NurseSpesfication(Expression<Func<Nures,bool>> expression):base(expression)
        {
        }
        public NurseSpesfication(ConfrmationStatus Status) : base(d => d.Status.Equals(Status)&&d.IsDeleted==false)
        {
        }
    }
}

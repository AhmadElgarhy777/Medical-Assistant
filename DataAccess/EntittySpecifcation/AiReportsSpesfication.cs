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
    public class AiReportsSpesfication:Specfication<AiReport>
    {
        public AiReportsSpesfication(Expression<Func<AiReport,bool>> expression):base(expression)
        {
            Includes.Add(p => p.Doctor);
            Includes.Add(p => p.Doctor.Specialization);
        }
        public AiReportsSpesfication():base()
        {
            Includes.Add(p => p.Doctor);
            Includes.Add(p => p.Doctor.Specialization);
        }
    }
}

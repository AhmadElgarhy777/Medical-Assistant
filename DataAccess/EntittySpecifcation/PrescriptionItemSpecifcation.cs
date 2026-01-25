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
    public class PrescriptionItemSpecifcation:Specfication<PrescriptionItem>
    {
        public PrescriptionItemSpecifcation(Expression<Func<PrescriptionItem,bool>> expression):base(expression)
        {
            
        }
    }
}

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
    public class PharmacySpecifcation: Specfication<Pharmacy>
    {
        public PharmacySpecifcation(Expression<Func<Pharmacy, bool>> expression) : base(expression)
        {
            
        }
        public PharmacySpecifcation() : base(p=>p.IsDeleted==false)
        {
           
        }
    
    }
}

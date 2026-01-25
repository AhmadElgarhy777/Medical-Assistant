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
    public class PresciptionSpecifcation:Specfication<Presciption>
    {
        public PresciptionSpecifcation(Expression<Func<Presciption,bool>> expression):base(expression)
        {
            Includes.Add(e => e.Doctor);
        }
        public PresciptionSpecifcation():base()
        {
            Includes.Add(e => e.Doctor);
        }
    }
}

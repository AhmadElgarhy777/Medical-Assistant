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
    public class RatingSpecifcation:Specfication<Rating>
    {
        public RatingSpecifcation(Expression<Func<Rating,bool>> expression):base(expression)
        {

        }
       
    }
}

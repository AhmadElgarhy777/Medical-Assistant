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
    public class CommentsSpecifcation:Specfication<Comment>
    {
        public CommentsSpecifcation(Expression<Func<Comment, bool>> expression) : base(expression)
        {
            
        }
    }
}

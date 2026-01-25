using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Specfications
{
    public interface ISpecifcation<T> where T : ModelBase
    {
        public Expression<Func<T,bool>>? Expresion { get; set; }
        public List<Expression<Func<T,object>>>? Includes { get; set; }

        public bool Tracking { get; set; }
    }
}

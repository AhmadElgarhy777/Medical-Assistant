using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Specfications
{
    public class Specfication<T> : ISpecifcation<T> where T : ModelBase
    {
        public Expression<Func<T, bool>>? Expresion { get; set; }
        public List<Expression<Func<T, object>>>? Includes { get; set; } = null;
        public bool Tracking { get; set; } = true;

        public Specfication()
        {
            Includes=new List<Expression<Func<T, object>>>();
        }
        public Specfication(Expression<Func<T, bool>> expresion)
        {
            Expresion=expresion;
            Includes = new List<Expression<Func<T, object>>>();
        }
    }
}

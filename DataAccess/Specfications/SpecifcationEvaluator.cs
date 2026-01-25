using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Specfications
{
    public class SpecifcationEvaluator<T>:Specfication<T> where T:ModelBase 
    {
       

        public static IQueryable<T> GetQuery(DbSet<T> dbset,ISpecifcation<T> spec)
        {
            IQueryable<T> query=dbset;

            if (spec.Expresion is not null) query = query.Where(spec.Expresion);

            query = spec.Includes.Aggregate(query, (currentquery, extentionquery) => currentquery.Include(extentionquery));
            
            if (!spec.Tracking) query=query.AsNoTracking();
            
            return query;
        }
    }
}

using DataAccess.Repositry.IRepositry;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class Repositry<T> : IRepositry<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;
        DbSet<T> dbset;

        public Repositry(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbset = dbContext.Set<T>();
        }
        public async Task AddAsync(T item)
        {
            await dbset.AddAsync(item);
        }
        public async Task AddRangeAsync(T item)
        {
            await dbset.AddRangeAsync(item);
        }

        public async Task CommitAsync(CancellationToken token = default)
        {
            await dbContext.SaveChangesAsync(token);
        }

        public void Delete(T item)
        {
            dbset.Remove(item);
        }
        public void DeleteRange(T item)
        {
            dbset.RemoveRange(item);
        }

        public void Edit(T item)
        {
            dbset.Update(item);
        }

        public IQueryable<T> GetAll(Expression<Func<T, object>>[]? includeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true)
        {
            IQueryable<T> qury = dbset;
            if (includeProp != null)
            {
                foreach (var item in includeProp)
                {
                    qury = qury.Include(item);
                }
            }
            if (expression != null)
            {
                qury = qury.Where(expression);
            }
            if (!tracked)
            {
                qury = qury.AsNoTracking();
            }
            return qury;
        }

        public async Task<T?> GetOneAsync(Expression<Func<T, object>>[]? includeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true, CancellationToken Token = default)
        {
            return await GetAll(includeProp, expression, tracked).FirstOrDefaultAsync(Token);
        }

        public async Task ExpLoadCollectionAsync<TProperty>
            (
                T item,
                Expression<Func<T, IEnumerable<TProperty>>> collection,
                                CancellationToken cancellationToken = default

            ) where TProperty : class
        {
            await dbset.Entry(item).Collection(collection).LoadAsync(cancellationToken);
        }
        public async Task ExpLoadRefrenceAsync<TProperty>
            (
                T item,
                Expression<Func<T, TProperty>> refrence = null!,
                CancellationToken cancellationToken = default
            ) where TProperty : class
        {
            await dbset.Entry(item).Reference(refrence).LoadAsync(cancellationToken);
        }
    }
}

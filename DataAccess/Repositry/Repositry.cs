using DataAccess.Repositry.IRepositry;
using DataAccess.Specfications;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class Repositry<T> : IRepositry<T> where T : ModelBase
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

       

        public IQueryable<T> GetAll(ISpecifcation<T> spec)
        {
            var specifcation = GetSpecifcation(spec);
            return specifcation;
        }
        public IQueryable<T?> GetOne(ISpecifcation<T> spec)
        {
            var specifcation = GetSpecifcation(spec);
            return specifcation;
        }

        private IQueryable<T> GetSpecifcation(ISpecifcation<T> spec)
        {
            return SpecifcationEvaluator<T>.GetQuery(dbset, spec);
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

using DataAccess.Specfications;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry.IRepositry
{
    public interface IRepositry<T> where T : ModelBase
    {
        //public IQueryable<T> GetAll(Expression<Func<T, object>>[]? includeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true);
        //public Task<T?> GetOneAsync(Expression<Func<T, object>>[]? includeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true, CancellationToken Token = default);
        public Task AddAsync(T item);
        public Task AddRangeAsync(T item);
        void Edit(T item);
        void Delete(T item);
        public void DeleteRange(T item);
        Task CommitAsync(CancellationToken token = default);

        public Task ExpLoadCollectionAsync<TProperty>
           (
               T item,
               Expression<Func<T, IEnumerable<TProperty>>> collection,
                               CancellationToken cancellationToken = default

           ) where TProperty : class;
        public Task ExpLoadRefrenceAsync<TProperty>
        (
        T item,
        Expression<Func<T, TProperty>> refrence = null!,
                        CancellationToken cancellationToken = default

         ) where TProperty : class;


        public IQueryable<T> GetAll(ISpecifcation<T> spec);
        public IQueryable<T?> GetOne(ISpecifcation<T> spec);
    }

}

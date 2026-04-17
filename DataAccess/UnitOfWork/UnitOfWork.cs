using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        public ValueTask DisposeAsync()
        {
            return dbContext.DisposeAsync();
        }
    }
}

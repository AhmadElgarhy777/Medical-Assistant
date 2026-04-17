using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public interface IUnitOfWork:IAsyncDisposable
    {
        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken=default);
        public Task CompleteAsync(CancellationToken cancellationToken=default);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;

namespace DataAccess.Repositry.IRepositry
{
    public interface ILabRepository
    {
        Task AddLabAsync(Lab lab);
        Task<Lab?> GetLabByIdAsync(string labId);
        Task UpdateLabAsync(Lab lab);
    }
}
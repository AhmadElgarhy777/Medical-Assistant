using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess.Repositry.IRepositry;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Repositry
{
    public class LabRepository : ILabRepository
    {
        private readonly ApplicationDbContext _context;
        public LabRepository(ApplicationDbContext context) => _context = context;

        public async Task AddLabAsync(Lab lab)
        {
            await _context.Labs.AddAsync(lab);
        }

        public async Task<Lab?> GetLabByIdAsync(string labId)
        {
            return await _context.Labs.FirstOrDefaultAsync(l => l.ID == labId);
        }

        public Task UpdateLabAsync(Lab lab)
        {
            _context.Labs.Update(lab);
            return Task.CompletedTask;
        }
    }
}
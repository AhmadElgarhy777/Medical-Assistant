using DataAccess.Repositry.IRepositry;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class SpecilizationRepositry:Repositry<Specialization>,ISpecilizationRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public SpecilizationRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

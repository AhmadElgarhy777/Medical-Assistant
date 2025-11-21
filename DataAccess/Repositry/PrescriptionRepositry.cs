using DataAccess.Repositry.IRepositry;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class PrescriptionRepositry:Repositry<Presciption>,IPresciptionRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public PrescriptionRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

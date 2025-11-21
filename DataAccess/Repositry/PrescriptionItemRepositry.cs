using DataAccess.Repositry.IRepositry;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class PrescriptionItemRepositry:Repositry<PrescriptionItem>,IPresciptionItemRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public PrescriptionItemRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

using DataAccess.Repositry.IRepositry;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class PharmacyReposities:Repositry<Pharmacy>,IPharmacyReposities
    {
        private readonly ApplicationDbContext dbContext;

        public PharmacyReposities(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

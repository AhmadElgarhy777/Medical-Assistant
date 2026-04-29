using DataAccess.Repositry.IRepositry;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class AdminRepositry : Repositry<Admin>, IAdminRepositry

    {
        private readonly ApplicationDbContext dbContext;

        public AdminRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

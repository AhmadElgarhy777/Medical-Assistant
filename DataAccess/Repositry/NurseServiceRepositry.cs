using DataAccess.Repositry.IRepositry;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class NurseServiceRepositry : Repositry<NurseService>, INurseServicesRepositry

    {
        private readonly ApplicationDbContext dbContext;

        public NurseServiceRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
    public class NurseingServiceRepositry : Repositry<NursingService>, INursingServicesRepositry

    {
        private readonly ApplicationDbContext dbContext;

        public NurseingServiceRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}


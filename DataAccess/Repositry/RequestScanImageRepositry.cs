using DataAccess.Repositry.IRepositry;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
     public class RequestScanImageRepositry : Repositry<RequestedScanImage>, IRequestScanImageRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public RequestScanImageRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

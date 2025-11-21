using DataAccess.Repositry.IRepositry;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class DoctorPatientRepositry:Repositry<DoctorPatient>,IDoctorPatientRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public DoctorPatientRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

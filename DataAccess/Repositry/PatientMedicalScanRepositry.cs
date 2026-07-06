using DataAccess.Repositry.IRepositry;
using Models;

namespace DataAccess.Repositry
{
    public class PatientMedicalScanRepositry : Repositry<PatientMedicalScan>, IPatientMedicalScanRepositry
    {
        private readonly ApplicationDbContext dbContext;

        public PatientMedicalScanRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

using DataAccess.Repositry.IRepositry;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositry
{
    public class RefreshTokenRepositry: Repositry<RefreshToken>, IRefreshTokenRepositry
    {
        public RefreshTokenRepositry(ApplicationDbContext dbContext):base(dbContext)
        {
            
        }
    }
}

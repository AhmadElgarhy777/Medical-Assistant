using DataAccess.Specfications;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntittySpecifcation
{
    public class RefreshTokenSpecifcation:Specfication<RefreshToken>
    {
        public RefreshTokenSpecifcation(string tokenHash):base(r=>r.TokenHash==tokenHash)
        {
            Includes.Add(e => e.User);     
        }
    }
}

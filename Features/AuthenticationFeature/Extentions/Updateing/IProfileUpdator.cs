using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Extentions.Updateing
{
    public interface IProfileUpdator
    {
        Task UpdateAsync (ApplicationUser user,UpdateProfileDTO userUpdate,CancellationToken cancellationToken);
    }
}

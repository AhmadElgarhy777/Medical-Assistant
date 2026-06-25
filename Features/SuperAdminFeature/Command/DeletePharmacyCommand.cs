using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.SuperAdminFeature.Command
{
    public record DeletePharmacyCommand(string PharmacyId) : IRequest<ResultResponse<bool>>;

}

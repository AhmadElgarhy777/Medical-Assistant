using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.DoctorFeature.Commands
{
    public record EditDoctorPriceCommand(string DocId,double price):IRequest<ResultResponse<bool>>;
    
}

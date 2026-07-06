using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Features.LabFeature.Commands
{
    public record AddLabTestOfferCommand(string MedicalTestId, decimal? Price) : IRequest<ResultResponse<string>>;
}
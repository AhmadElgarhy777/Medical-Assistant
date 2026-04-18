using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Command
{
    public record CreateCommentCommand(string PatientId,string TargetId, string Comment):IRequest<ResultResponse<String>>;
   
}

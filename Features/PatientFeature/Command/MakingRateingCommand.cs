using MediatR;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Command
{
    public record MakingCommentCommand(StarsRatingEnum Rateing, string TargetId, string PatientId, string? Comment = null):IRequest<ResultResponse<String>>;
   
}

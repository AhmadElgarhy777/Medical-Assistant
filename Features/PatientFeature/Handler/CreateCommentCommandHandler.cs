using DataAccess.EntittySpecifcation;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.PatientFeature.Command;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Features.PatientFeature.Handler
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, ResultResponse<String>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICommentRepositry commentRepositry;

        public CreateCommentCommandHandler(UserManager<ApplicationUser> userManager,ICommentRepositry commentRepositry)
        {
            this.userManager = userManager;
            this.commentRepositry = commentRepositry;
        }
        public async Task<ResultResponse<string>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var TargetId = request.TargetId;
            var patientId = request.PatientId;
            var commentTxt = request.Comment;

            var user = await userManager.FindByIdAsync(TargetId);
            if (user is null)
            {
                return new ResultResponse<String>
                {
                    ISucsses = false,
                    Message = ",Can Not Make Commint , The User Is Not Found "
                };
            }
            if(commentTxt is null || commentTxt.Trim().Length == 0)
            {
                return new ResultResponse<String>
                {
                    ISucsses = false,
                    Message = ",Comment Can Not Be Empty "
                };
            }
            var acceptRoles = new[] { SD.DoctorRole, SD.NurseRole, SD.PharmacyRole };
            if (!acceptRoles.Contains(user.Role))
            {
                return new ResultResponse<String>
                {
                    ISucsses = false,
                    Message = ",This User Can Not have a comment "
                };
            }
           
            var comment = new Comment
            {
                TargetId = TargetId,
                PatientId = patientId,
                CommentText = commentTxt,
                CreatedAt = DateTime.Now,
                TargetRole = user.Role

            };

            commentRepositry.Add(comment);
            await commentRepositry.CommitAsync();

            return new ResultResponse<String>
            {
                ISucsses = true,
                Message = "Your Comment Has Been Saved"
            };
        }
    }
}

using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using Features.AuthenticationFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Features.AuthenticationFeature.Handlers
{
    internal class GetAllCommentsForTargetCommandHandler : IRequestHandler<GetAllCommentsForTargetCommand, ResultResponse<List<ShowCommentDTO>>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICommentRepositry commentRepositry;

        public GetAllCommentsForTargetCommandHandler(UserManager<ApplicationUser> userManager, ICommentRepositry commentRepositry)
        {
            this.userManager = userManager;
            this.commentRepositry = commentRepositry;
        }
        public async Task<ResultResponse<List<ShowCommentDTO>>> Handle(GetAllCommentsForTargetCommand request, CancellationToken cancellationToken)
        {
            var targetId = request.TargetId;
            var user = await userManager.FindByIdAsync(targetId);
            if (user == null)
            {
                return new ResultResponse<List<ShowCommentDTO>>
                {
                    ISucsses = false,
                    Message = "User not found",

                };
            }
            if(user.Role == SD.PatientRole)
            {
                return new ResultResponse<List<ShowCommentDTO>>
                {
                    ISucsses = false,
                    Message = "Patient cannot have comments",
                };
            }

            var spec = new CommentsSpecifcation(c => c.TargetId == targetId);
            var commentsList = await commentRepositry.GetAll(spec).ToListAsync();
            var commentsDTOList = commentsList.Select(c => new ShowCommentDTO
            {
                PatientId = c.PatientId,
                PatientName = userManager.FindByIdAsync(c.PatientId).Result.UserName,
                Comment = c.CommentText,
                CreatedAt = c.CreatedAt
            }).ToList();
            return new ResultResponse<List<ShowCommentDTO>>
            {
                ISucsses = true,
                Message = "Comments retrieved successfully",
                Obj = commentsDTOList

            };
        }
    }
}

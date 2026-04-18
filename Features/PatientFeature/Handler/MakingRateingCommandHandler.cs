using DataAccess;
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.PatientFeature.Command;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Features.PatientFeature.Handler
{
    public class MakingRateingCommandHandler : IRequestHandler<MakingCommentCommand, ResultResponse<String>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRatingRepositry ratingRepositry;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IDoctorRepositry doctorRepositry;
        private readonly INuresRepositry nuresRepositry;
        private readonly IPharmacyRepository pharmacyRepository;

        public MakingRateingCommandHandler(UserManager<ApplicationUser> userManager,
            IRatingRepositry ratingRepositry,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            IDoctorRepositry doctorRepositry,
            INuresRepositry nuresRepositry,
            IPharmacyRepository pharmacyRepository
            )
        {
            this.userManager = userManager;
            this.ratingRepositry = ratingRepositry;
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
            this.doctorRepositry = doctorRepositry;
            this.nuresRepositry = nuresRepositry;
            this.pharmacyRepository = pharmacyRepository;
        }
        public async Task<ResultResponse<String>> Handle(MakingCommentCommand request, CancellationToken cancellationToken)
        {
            var targetId=request.TargetId;
            var patientId = request.PatientId;
            var stars=request.Rateing;
            var comment = request.Comment;
            var user = await userManager.FindByIdAsync(targetId);
            if(user is null)
            {
                return new ResultResponse<String>
                {
                    ISucsses = false,
                    Message = ",Can Not Be Rated, The User Is Not Found "
                };
            }

            var acceptRoles = new []{ SD.DoctorRole, SD.NurseRole, SD.PharmacyRole };
            if (!acceptRoles.Contains(user.Role))
            {
                return new ResultResponse<String>
                {
                    ISucsses = false,
                    Message = ",This User Can Not Be Rated "
                };
            }
            var spec=new RatingSpecifcation(r=>r.TargetId==user.Id && r.PatientId==patientId);
            var IsDublicatedRateing = await ratingRepositry.GetOne(spec).FirstOrDefaultAsync();
            if (IsDublicatedRateing is not null)
            {
                if(IsDublicatedRateing.Stars == stars)
                {
                    return new ResultResponse<String>
                    {
                        ISucsses = false,
                        Message = "You Have Already Rated This User With The Same Stars"
                    };
                }
                else
                {
                    IsDublicatedRateing.Stars = stars;
                    if(comment is not null) IsDublicatedRateing.Comment = comment;
                    ratingRepositry.Edit(IsDublicatedRateing);
                    await unitOfWork.CompleteAsync(cancellationToken); // ✅ save edit first

                    await UpdateAverageRatingAsync(targetId, user.Role, cancellationToken); // ✅ recalculate
                    await unitOfWork.CompleteAsync(cancellationToken); // ✅ save updated average
                    return new ResultResponse<String>
                    {
                        ISucsses = true,
                        Message = "Your Rating Has Been Saved"
                    };
                }
            }

            var rating = new Rating
            {
                Stars = stars,
                Comment= comment,
                PatientId = patientId,
                TargetId = user.Id,
                TargetRole = user.Role,
                CreatedAt = DateTime.Now
            };

            ratingRepositry.Add(rating);
            await unitOfWork.CompleteAsync(cancellationToken); // ✅ save rating first

            await UpdateAverageRatingAsync(targetId, user.Role, cancellationToken); // ✅ now avg includes new rating
            await unitOfWork.CompleteAsync(cancellationToken); // ✅ 

            return new ResultResponse<String>
            {
                ISucsses = true,
                Message = "Your Rating Has Been Saved"
            };
        }
        private async Task UpdateAverageRatingAsync(string targetId, string targetRole, CancellationToken cancellationToken)
        {
           var averageRating = await mediator.Send(new CalcAverageRateQuery(targetId), cancellationToken);

            switch (targetRole)
            {
                case SD.DoctorRole:
                    var Docspec=new DoctorSpecifcation(targetId);
                    var doctor = await doctorRepositry.GetOne(Docspec).FirstOrDefaultAsync();
                    doctor.RattingAverage = averageRating;
                    break;

                case SD.NurseRole:
                    var nuresSpec=new NurseSpesfication(targetId);
                    var nurse = await nuresRepositry.GetOne(nuresSpec).FirstOrDefaultAsync();
                    nurse.RattingAverage = averageRating;
                    break;

                case SD.PharmacyRole:
                    var pharmacy = await pharmacyRepository.GetPharmacyByIdAsync(targetId);
                    pharmacy.RattingAverage = averageRating;
                    break;
            }
        }
    }
}

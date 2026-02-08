using DataAccess.EntittySpecifcation;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.RegisterationFeature.Events.Event;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTOs;
using Services.ImageServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Extentions.Updateing
{
    public class NurseProfileUpdate : IProfileUpdator
    {
        private readonly INuresRepositry nurseRepositry;
        private readonly IMediator mediator;
        private readonly IImageService imageService;
        private readonly IConfiguration configration;

        public NurseProfileUpdate(INuresRepositry NurseRepositry,
            IMediator mediator,
            IImageService imageService,
            IConfiguration configration)
        {
            nurseRepositry = NurseRepositry;
            this.mediator = mediator;
            this.imageService = imageService;
            this.configration = configration;
        }
        public async Task UpdateAsync(ApplicationUser user, UpdateProfileDTO userUpdate, CancellationToken cancellationToken)
        {
            var Nspec = new NurseSpesfication(user.Id);
            var nurse = await nurseRepositry.GetOne(Nspec).FirstOrDefaultAsync();

            if (nurse is not null)
            {
                if (!string.IsNullOrWhiteSpace(userUpdate.UserName))
                {
                    user.UserName = userUpdate.UserName;
                    nurse.UserName = userUpdate.UserName;

                }

                if (!string.IsNullOrWhiteSpace(userUpdate.Address))
                {
                    user.Address = userUpdate.Address;
                    nurse.Address = userUpdate.Address;
                }

                if (!string.IsNullOrWhiteSpace(userUpdate.City))
                {
                    user.City = userUpdate.City;
                    nurse.City = userUpdate.City;
                }
                if (userUpdate.Governorate != nurse.Governorate)
                {
                    nurse.Governorate = userUpdate.Governorate;
                    user.Governorate = userUpdate.Governorate;
                }
                
                if (userUpdate.Email is not null)
                {
                    nurse.Email = userUpdate.Email;
                    user.Email = userUpdate.Email;
                    user.EmailConfirmed = false;
                    await mediator.Publish(new ConfirmEmailEvent(user.Id, userUpdate.Email, cancellationToken));
                }
                if (userUpdate.Img is not null)
                {
                    var newImage = await imageService.EditImgAsync(userUpdate.Img, nurse.Img, "NurseImages/ProfileImages", cancellationToken);
                    nurse.Img = $"{configration["ApiBaseUrl"]}/NurseImages/ProfileImages/{newImage}";
                    user.Img = $"{configration["ApiBaseUrl"]}/NurseImages/ProfileImages/{newImage}";
                }

                nurseRepositry.Edit(nurse);
                await nurseRepositry.CommitAsync(cancellationToken);

            }
        }
    }
}

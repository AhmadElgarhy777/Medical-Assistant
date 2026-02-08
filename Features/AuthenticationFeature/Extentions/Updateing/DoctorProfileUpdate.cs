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
    public class DoctorProfileUpdate : IProfileUpdator
    {
        private readonly IDoctorRepositry doctorRepositry;
        private readonly IMediator mediator;
        private readonly IImageService imageService;
        private readonly IConfiguration configration;

        public DoctorProfileUpdate(IDoctorRepositry doctorRepositry,
            IMediator mediator,
            IImageService imageService,
            IConfiguration configration)
        {
            this.doctorRepositry = doctorRepositry;
            this.mediator = mediator;
            this.imageService = imageService;
            this.configration = configration;
        }
        public async Task UpdateAsync(ApplicationUser user, UpdateProfileDTO userUpdate, CancellationToken cancellationToken)
        {
            var Dspec = new DoctorSpecifcation(user.Id);
            var doctor = await doctorRepositry.GetOne(Dspec).FirstOrDefaultAsync(cancellationToken);
            if (doctor is not null)
            {
                if (!string.IsNullOrWhiteSpace(userUpdate.UserName))
                {
                    user.UserName = userUpdate.UserName;
                    doctor.UserName = userUpdate.UserName;

                }

                if (!string.IsNullOrWhiteSpace(userUpdate.Address))
                {
                    user.Address = userUpdate.Address;
                    doctor.Address = userUpdate.Address;
                }

                if (!string.IsNullOrWhiteSpace(userUpdate.City))
                {
                    user.City = userUpdate.City;
                    doctor.City = userUpdate.City;
                }
                if (userUpdate.Governorate != doctor.Governorate)
                {
                    doctor.Governorate = userUpdate.Governorate;
                    user.Governorate = userUpdate.Governorate;
                }
                
                if (userUpdate.Email is not null)
                {
                    doctor.Email = userUpdate.Email;
                    user.Email = userUpdate.Email;
                    user.EmailConfirmed = false;
                    await mediator.Publish(new ConfirmEmailEvent(user.Id, userUpdate.Email, cancellationToken));
                }
                if (userUpdate.Img is not null)
                {
                    var newImage = await imageService.EditImgAsync(userUpdate.Img, doctor.Img, "DoctorImages/ProfileImages", cancellationToken);
                    doctor.Img = $"{configration["ApiBaseUrl"]}/DoctorImages/ProfileImages/{newImage}";
                    user.Img = $"{configration["ApiBaseUrl"]}/DoctorImages/ProfileImages/{newImage}";
                }

                doctorRepositry.Edit(doctor);
                await doctorRepositry.CommitAsync(cancellationToken);
            }
         
        }
    }
}

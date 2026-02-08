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
    public class PatientProfileUpdate : IProfileUpdator
    {
        private readonly IPatientRepositry patientRepositry;
        private readonly IMediator mediator;
        private readonly IImageService imageService;
        private readonly IConfiguration configration;

        public PatientProfileUpdate(IPatientRepositry patientRepositry,
            IMediator mediator,
            IImageService imageService,
            IConfiguration configration)
        {
            this.patientRepositry = patientRepositry;
            this.mediator = mediator;
            this.imageService = imageService;
            this.configration = configration;
        }
        public async Task UpdateAsync(ApplicationUser user, UpdateProfileDTO userUpdate, CancellationToken cancellationToken)
        {
            var Pspec = new PatientSpecifcation(user.Id);
            var patient = await patientRepositry.GetOne(Pspec).FirstOrDefaultAsync();
            if (patient is not null)
            {
                if (!string.IsNullOrWhiteSpace(userUpdate.UserName))
                {
                    user.UserName = userUpdate.UserName;
                    patient.UserName = userUpdate.UserName;

                }

                if (!string.IsNullOrWhiteSpace(userUpdate.Address))
                {
                    user.Address = userUpdate.Address;
                    patient.Address = userUpdate.Address;
                }

                if (!string.IsNullOrWhiteSpace(userUpdate.City))
                {
                    user.City = userUpdate.City;
                    patient.City = userUpdate.City;
                }
                if (userUpdate.Governorate!= patient.Governorate)
                {
                    patient.Governorate = userUpdate.Governorate;
                    user.Governorate = userUpdate.Governorate;
                }

                
                if (userUpdate.Email is not null)
                {
                    patient.Email = userUpdate.Email;
                    user.Email = userUpdate.Email;
                    user.EmailConfirmed = false;
                    await mediator.Publish(new ConfirmEmailEvent(user.Id, userUpdate.Email, cancellationToken));
                }
                if (userUpdate.Img is not null)
                {
                    var newImage = await imageService.EditImgAsync(userUpdate.Img, patient.Img, "PatientImages", cancellationToken);
                    patient.Img = $"{configration["ApiBaseUrl"]}/PatientImages/{newImage}";
                    user.Img = $"{configration["ApiBaseUrl"]}/PatientImages/{newImage}";
                }

                patientRepositry.Edit(patient);
                await patientRepositry.CommitAsync(cancellationToken);
            }
         
        }
    }
}

using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using Features.AdminFeature.Commands;
using Features.RegisterationFeature.Events.Event;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using Services.EmailServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Features.AdminFeature.Handlers
{
    public class ChangeStatusHandler : IRequestHandler<ChangeStatusCommand, ResultResponse<String>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDoctorRepositry doctorRepositry;
        private readonly INuresRepositry nuresRepositry;
        private readonly IEmailServices emailServices;

        public ChangeStatusHandler(UserManager<ApplicationUser> userManager,
            IDoctorRepositry doctorRepositry,
            INuresRepositry nuresRepositry,
            IEmailServices emailServices)
        {
            this.userManager = userManager;
            this.doctorRepositry = doctorRepositry;
            this.nuresRepositry = nuresRepositry;
            this.emailServices = emailServices;
        }
        public async Task<ResultResponse<string>> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
        {
            var Id = request.userId;
            var Status = request.status;

            var user = await userManager.FindByIdAsync(Id);
            if (user is not null)
            {
                if (user.Role == SD.DoctorRole)
                {
                    var spec = new DoctorSpecifcation(Id);
                    var doctor = await doctorRepositry.GetOne(spec).FirstOrDefaultAsync(cancellationToken);
                    if (doctor is not null)
                    {
                        if (doctor.Status.Equals(ConfrmationStatus.Approved))
                        {
                            return new ResultResponse<string>
                            {
                                ISucsses = false,
                                Message = "The user already approved"
                            };
                        }
                        if (Status.Equals(ConfrmationStatus.Approved))
                        {
                            doctor.Status = ConfrmationStatus.Approved;
                            doctorRepositry.Edit(doctor);
                            await doctorRepositry.CommitAsync(cancellationToken);
                            await emailServices.SendEmailAsync(doctor.Email, "Approved Your Email",
                                $"Congratulations Doctor {doctor.UserName}, \n Your Account has been reviewed and approved sucessfully , \n now you can go and login ");
                        }
                        else if (Status.Equals(ConfrmationStatus.Rejected))
                        {
                            doctor.Status = ConfrmationStatus.Rejected;
                            doctorRepositry.Edit(doctor);
                            await doctorRepositry.CommitAsync(cancellationToken);

                            await emailServices.SendEmailAsync(doctor.Email, "Your Status",
                                $"Dear {doctor.UserName}, \n Your Account has been Rejected and not approved, \nContact with as for more informations");
                            //احذفه ولاا لا !!!
                        }

                        else
                        {
                            doctor.Status = ConfrmationStatus.Pending;
                            doctorRepositry.Edit(doctor);
                            await doctorRepositry.CommitAsync(cancellationToken);

                        }

                        return new ResultResponse<string>
                        {
                            ISucsses = true,
                            Message = "The stauts changed succefully"
                        };

                    }
                }

                else if (user.Role == SD.NurseRole)
                {
                    var Spec = new NurseSpesfication(Id);
                    var nurse = await nuresRepositry.GetOne(Spec).FirstOrDefaultAsync(cancellationToken);
                    if (nurse is not null)
                    {
                        if (nurse.Status.Equals(ConfrmationStatus.Approved))
                        {
                            return new ResultResponse<string>
                            {
                                ISucsses = false,
                                Message = "The user already approved"
                            };
                        }
                        if (Status.Equals(ConfrmationStatus.Approved))
                        {
                            nurse.Status = ConfrmationStatus.Approved;
                            nuresRepositry.Edit(nurse);
                            await nuresRepositry.CommitAsync(cancellationToken);

                            await emailServices.SendEmailAsync(nurse.Email, "Approved Your Email",
                                $"Congratulations Doctor {nurse.UserName}, \n Your Account has been reviewed and approved sucessfully , \n now you can go and login ");
                        }
                        else if (Status.Equals(ConfrmationStatus.Rejected))
                        {
                            nurse.Status = ConfrmationStatus.Rejected;
                            nuresRepositry.Edit(nurse);
                            await nuresRepositry.CommitAsync(cancellationToken);

                            await emailServices.SendEmailAsync(nurse.Email, "Your Status",
                                $"Dear {nurse.UserName}, \n Your Account has been Rejected and not approved, \nContact with as for more informations");
                            //احذفه ولاا لا !!!
                        }

                        else
                        {
                            nurse.Status = ConfrmationStatus.Pending;
                            nuresRepositry.Edit(nurse);
                            await nuresRepositry.CommitAsync(cancellationToken);


                        }

                        return new ResultResponse<string>
                        {
                            ISucsses = true,
                            Message = "The stauts changed succefully"
                        };
                    }
                }

                else
                {
                    return new ResultResponse<string>
                    {
                        ISucsses = false,
                        Message = "The user is invalid"
                    };
                }

            }
            return new ResultResponse<string>
            {
                ISucsses = false,
                Message = "The user is not found"
            };
        }
    }
}

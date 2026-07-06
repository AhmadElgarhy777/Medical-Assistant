using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using Features.PatientFeature.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Handler
{
    internal class GetNurseSearchDetailsByIdHandler : IRequestHandler<GetNurseDetailsByIdQuery, ResultResponse<SearchNurseDetailsResultByIdDto>>
    {
        private readonly INuresRepositry nuresRepositry;

        public GetNurseSearchDetailsByIdHandler(INuresRepositry nuresRepositry)
        {
            this.nuresRepositry = nuresRepositry;
        }
        public async Task<ResultResponse<SearchNurseDetailsResultByIdDto>> Handle(GetNurseDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new NurseSpesfication(request.NurseId);
            var nures = await nuresRepositry.GetAll(spec).FirstOrDefaultAsync(cancellationToken);

            if (nures == null )
            {
                return new ResultResponse<SearchNurseDetailsResultByIdDto>
                {
                    ISucsses = false,
                    Message = "Nurse not found,Invalid ID   ",

                };

            }

            var ServiceForNurse = new List<NurseServiceDto>();
            foreach (var service in nures.NurseServices)
            {
                ServiceForNurse.Add(new NurseServiceDto { Id = service.Service.ID, Name = service.Service.Name });
                
            }

            var result = new SearchNurseDetailsResultByIdDto
            {
                ID = nures.ID,
                FullName = nures.FullName,
                UserName = nures.UserName,
                Address = nures.Address,
                Age = DateTime.Now.Year - nures.BD.Year,
                Bio = nures.Bio,
                City = nures.City,
                Degree = nures.Degree,
                Email = nures.Email,
                Experence = nures.Experence,
                Gender = nures.Gender,
                Governorate = nures.Governorate,
                Img = nures.Img,
                NurseSpecialty = nures.NurseSpecialty,
                Phone = nures.Phone,
                PricePerHours = nures.PricePerHours,
                RattingAverage = nures.RattingAverage,
                WorkAt = nures.WorkAt,
                nurseServiceDtos = ServiceForNurse
            };

            return new ResultResponse<SearchNurseDetailsResultByIdDto>
            {
                ISucsses = true,
                Message = "Nurse found successfully",
                Obj = result
            };
        }
    }
}
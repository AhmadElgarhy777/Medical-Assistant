using AutoMapper;
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using Features.PatientFeature.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Handler
{
    public class GetPrescriptionHandler : IRequestHandler<GetPresciptionQuery, List<PresciptionDTO>>
    {
        private readonly IPresciptionRepositry presciptionRepositry;
        private readonly IMapper mapper;

        public GetPrescriptionHandler(IPresciptionRepositry presciptionRepositry,IMapper mapper)
        {
            this.presciptionRepositry = presciptionRepositry;
            this.mapper = mapper;
        }
        public  async Task<List<PresciptionDTO>> Handle(GetPresciptionQuery request, CancellationToken cancellationToken)
        {
            var page = request.page;
            var patientId = request.patientId;
            var spec = new PresciptionSpecifcation(e => e.PatientId == patientId);
            var Presciptions = presciptionRepositry.GetAll(spec);
            Presciptions = Presciptions.Skip((page - 1) * 5).Take(5);
            List<Prescription> PresciptionsList = await Presciptions.ToListAsync(cancellationToken);
            var presciptionDTOs=mapper.Map<List<Prescription>,List<PresciptionDTO>>(PresciptionsList);
            return presciptionDTOs;


        }
    }
}

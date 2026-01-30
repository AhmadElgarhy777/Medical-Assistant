using MediatR;
using Models.DTOs;

namespace Features.PatientFeature.Queries
{
    public record GetFullPrescriptionQuery(string PrescriptionId) : IRequest<PrescriptionFullDetailsDto?>;
}
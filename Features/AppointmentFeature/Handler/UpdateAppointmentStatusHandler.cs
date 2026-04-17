using DataAccess;
using Features.AppointmentFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using System.Security.Claims;

namespace Features.AppointmentFeature.Handlers
{
    public class UpdateAppointmentStatusHandler : IRequestHandler<UpdateAppointmentStatusCommand, ResultResponse<String>>
    {
        private readonly ApplicationDbContext _context;

        public UpdateAppointmentStatusHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<String>> Handle(UpdateAppointmentStatusCommand request, CancellationToken cancellationToken)
        {
            // 1. بندور على الحجز في الداتابيز
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(x => x.ID == request.AppointmentId, cancellationToken);

            if(appointment.Status.Equals(bookStatusEnum.Completed) || appointment.Status.Equals(bookStatusEnum.Cancelled))
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = $"status can not be changed becouse it is {request.NewStatus}"
                };
            }
            if (appointment == null) 
                return new ResultResponse<string>
            {
                ISucsses = false,
                Message = $"appoinment can not be found"
            }; ;

            if (request.NewStatus.Equals(bookStatusEnum.Completed) || request.NewStatus.Equals(bookStatusEnum.Cancelled))
            {
               var docID= request.docId;
                var booktime = _context.DoctorAvilableTimes
                    .Where(a => a.DoctorId==docID && a.ID==appointment.SlotId)
                    .FirstOrDefault();

                booktime.IsBooked = false;
            }



            // 2. بنحدث الحالة للحالة الجديدة اللي الدكتور بعتها
            appointment.Status = request.NewStatus;


            // 3. بنسمّع في الداتابيز
            var result = await _context.SaveChangesAsync(cancellationToken);

            return  new ResultResponse<string>
            {
                ISucsses = true,
                Message = $"تم تحديث حالة الموعد بنجاح"
            }; ;
        }
    }
}

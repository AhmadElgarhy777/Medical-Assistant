using AutoMapper;
using DataAccess;
using Features;
using Features.DoctorFeature.Commands;
using Features.NurseFeature.Command;
using Features.NurseFeature.Handler;
using Features.PharmacyFeature;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Models.Enums;
using System.Security.Claims;
using Utility;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Authorize(Roles =$"{SD.NurseRole}")]
    public class NurseController : ApiBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly IPharmacyService pharmacyService;

        public NurseController(ApplicationDbContext context, IMapper mapper,IMediator mediator, IPharmacyService pharmacyService )
        {
            _context = context;
            this.mapper = mapper;
            this.mediator = mediator;
            this.pharmacyService = pharmacyService;
        }
       
        [HttpGet("pending-bookings")]
        [ProducesResponseType(typeof(BookingDTO),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPendingBookings()
        {
            var nurseId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(nurseId))
                return BadRequest("NurseId مطلوب.");

            var bookings = await _context.Bookings
                .Where(b => b.NurseId == nurseId && b.Status == bookStatusEnum.Pending.ToString())
                .ToListAsync();

            var bookingsListDTO = new List<BookingDTO>();
            foreach (var booking in bookings)
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.ID == booking.PatientId);

                bookingsListDTO.Add(new BookingDTO
                {
                    PatientId = booking.PatientId,
                    BookingID = booking.ID,
                    PatientName = patient.FullName,
                    Age = DateTime.UtcNow.Year - patient.BD.Year,
                    Address = patient.Address,
                    City = patient.City,
                    Governorate = patient.Governorate.ToString(),
                    RequestDate = booking.RequestDate,
                    PatientEmail=patient.Email,
                    bookDetails=booking.bookDetails,
                    BookingAddressNote=booking.AddressNote,
                    BookingLatitude=booking.Latitude,
                    BookingLongitude=booking.Longitude
                });
            }

            return Ok(bookingsListDTO);
        }


        [HttpGet("Confirmed-bookings")]
        [ProducesResponseType(typeof(BookingDTO), StatusCodes.Status200OK)]

        public async Task<IActionResult> GetConfirmedBookings()
        {
            var nurseId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var bookings = await _context.Bookings
                .Where(b => b.NurseId == nurseId && b.Status == bookStatusEnum.Confirmed.ToString())
                .ToListAsync();
            var bookingsListDTO = new List<BookingDTO>();
            foreach (var booking in bookings)
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.ID == booking.PatientId);
                bookingsListDTO.Add(new BookingDTO
                {
                    PatientId = booking.PatientId,
                    BookingID = booking.ID,
                    PatientName = patient.FullName,
                    Age = DateTime.UtcNow.Year - patient.BD.Year,
                    Address = patient.Address,
                    City = patient.City,
                    Governorate = patient.Governorate.ToString(),
                    RequestDate = booking.RequestDate,
                    PatientEmail=patient.Email,
                    bookDetails = booking.bookDetails,
                    BookingAddressNote = booking.AddressNote,
                    BookingLatitude = booking.Latitude,
                    BookingLongitude = booking.Longitude


                });
            }

            if (bookingsListDTO.Any())
            {
                return Ok(bookingsListDTO);


            }

            return NotFound("مافيش طلبات مستنية حاليا");
        }


        [HttpGet("Completed-bookings")]
        [ProducesResponseType(typeof(BookingDTO), StatusCodes.Status200OK)]

        public async Task<IActionResult> GetCompletedBookings()
        {
            var nurseId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var bookings = await _context.Bookings
                .Where(b => b.NurseId == nurseId && b.Status == bookStatusEnum.Completed.ToString())
                .ToListAsync();
            var bookingsListDTO = new List<BookingDTO>();
            foreach (var booking in bookings)
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.ID == booking.PatientId);
                bookingsListDTO.Add(new BookingDTO
                {
                    PatientId = booking.PatientId,
                    BookingID = booking.ID,
                    PatientName = patient.FullName,
                    Age = DateTime.UtcNow.Year - patient.BD.Year,
                    Address = patient.Address,
                    City = patient.City,
                    Governorate = patient.Governorate.ToString(),
                    RequestDate = booking.RequestDate,
                    PatientEmail = patient.Email,
                    bookDetails = booking.bookDetails,
                    BookingAddressNote = booking.AddressNote,
                    BookingLatitude = booking.Latitude,
                    BookingLongitude = booking.Longitude


                });
            }

            if (bookingsListDTO.Any())
            {
                return Ok(bookingsListDTO);


            }

            return NotFound("مافيش طلبات مستنية حاليا");


        }


        [HttpPut("UpdateBookingStatus")]
        [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]

        public async Task<IActionResult> UpdateBookingStatus(
        string bookingId,
        [FromForm] bookStatusEnum request)
        {
            // تحقق من وجود الحجز
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.ID == bookingId);

            if (booking == null)
                return NotFound("الحجز غير موجود");

            // تحقق من صحة الحالة باستخدام Enum
            if (!Enum.IsDefined(typeof(bookStatusEnum), request))
                return BadRequest("قيمة الحالة غير صحيحة");

            // تحديث الحالة
            booking.Status = request.ToString();

            await _context.SaveChangesAsync();

            // إرجاع الحجز بعد التحديث
            return Ok(booking);
        }


       
        [HttpPut("booking-complete/{bookingId}")]
        [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]

        public async Task<IActionResult> CompleteBooking(string bookingId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.ID == bookingId);

            if (booking == null)
                return NotFound();

            if (booking.Status != bookStatusEnum.Confirmed.ToString())
                return BadRequest("لازم الحجز يكون Confirmed");

            booking.Status = bookStatusEnum.Completed.ToString(); // هنا استخدمنا Enum بدل string

            await _context.SaveChangesAsync();
            return Ok(booking);
        }

        

        // 6️⃣ بروفايل الممرض
        [HttpGet("profile")]
        [ProducesResponseType(typeof(NurseDTO), StatusCodes.Status200OK)]

        public async Task<IActionResult> GetNurseProfile()
        {
            var nurseId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var nurse = await _context.Nures
                .FirstOrDefaultAsync(n => n.ID == nurseId);
            if (nurse is not null)
            {
                var nurseDTO = mapper.Map<Nures, NurseDTO>(nurse);
                return Ok(nurseDTO);
            }
            return NotFound();


        }

        [HttpPut("EditPrice")]
        [ProducesResponseType(typeof(ResultResponse<bool>), StatusCodes.Status200OK)]

        public async Task<IActionResult> EditPrice([FromQuery] decimal NewPrice,CancellationToken cancellationToken)
        {
            var nurseId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new EditNursePriceCommand(nurseId,NewPrice),cancellationToken);
            if (result.ISucsses)
            {
                return Ok(result.ISucsses);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllServicesForNurse")]
        public async Task<IActionResult> GetAllNursingServices( CancellationToken cancellationToken)
        {
            var nurseId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new GetAllNursingServicesQuery(nurseId), cancellationToken);
            if (result.ISucsses)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("GetAllServices")]
        public async Task<IActionResult> GetAllServices( CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetAllServicesQuery(), cancellationToken);
            if (result.ISucsses)
            {
                return Ok(result.Obj);
            }
            return BadRequest(result.Obj);
        }

        [HttpGet("AddServicesForNurse")]
        public async Task<IActionResult> AddServices([FromQuery]List<string> ServicesIds, CancellationToken cancellationToken)
        {
            var nurseId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new AddNurseServicesCommand(nurseId, ServicesIds), cancellationToken);
            if (result.ISucsses)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("RemoveServicesForNurse")]
        public async Task<IActionResult> RemoveServicesForNurse([FromQuery]string ServicesId, CancellationToken cancellationToken)
        {
            var nurseId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new RemoveNurseServiceCommand(nurseId, ServicesId), cancellationToken);
            if (result.ISucsses)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPut("profile/update/{nurseId}")]
        [Authorize(Roles = "Nurse")]
        public async Task<IActionResult> UpdateNurseProfile(string nurseId, [FromBody] UpdateNurseDto dto)
        {
            var result = await pharmacyService.UpdateNurseProfileAsync(nurseId, dto);
            if (!result)
                return NotFound("الممرض مش موجود!");
            return Ok("تم تعديل البروفايل بنجاح!");
        }
    }




    public class StatusRequest
    {
        public string Status { get; set; }
    }
}

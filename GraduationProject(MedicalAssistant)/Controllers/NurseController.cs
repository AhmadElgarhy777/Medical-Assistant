using AutoMapper;
using DataAccess;
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

        public NurseController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
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
                    PatientEmail=patient.Email
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
                    PatientEmail=patient.Email
                    


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
                    PatientEmail = patient.Email


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
                .Include(n => n.Ratings)
                .FirstOrDefaultAsync(n => n.ID == nurseId);
            if (nurse is not null)
            {
                var nurseDTO = mapper.Map<Nures, NurseDTO>(nurse);
                return Ok(nurseDTO);
            }
            return NotFound();


        }
    }




    public class StatusRequest
    {
        public string Status { get; set; }
    }
}

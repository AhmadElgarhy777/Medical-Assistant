using DataAccess;
using Features;
using Features.PatientFeature.Command;
using Features.PatientFeature.Queries;
using Features.PatientFeature.Query;
using Features.PharmacyFeature;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Enums;
using System.Security.Claims;
using System.Threading.Tasks;
using Utility;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Authorize(Roles = $"{SD.AdminRole},{SD.PatientRole}")]
    public class PatientController : ApiBaseController
    {
        private readonly IMediator mediatR;
        private readonly ApplicationDbContext dbContext;
        private readonly IPharmacyService _pharmacyService;
        private readonly IOrderService orderService;

        public PatientController(IMediator mediatR,ApplicationDbContext dbContext,
            IPharmacyService pharmacyService
            ,IOrderService orderService)
        {
            this.mediatR = mediatR;
            this.dbContext = dbContext;
            _pharmacyService = pharmacyService;
            this.orderService = orderService;
        }

        [HttpGet("GetDoctorsBySearch")]
        [ProducesResponseType(typeof(DoctorDTO),StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<List<DoctorDTO>>>> GetDoctorsBySearch([FromQuery] GetAllDoctorsSearchQuery query ,CancellationToken cancellationToken)
        {
                var doctors = await mediatR.Send(query, cancellationToken);
                if (doctors.ISucsses)
                {
                    return Ok(doctors.Obj);
                }
                return BadRequest(doctors.Message);
            
        }
        [HttpGet("GetNurseBySearch")]
        [ProducesResponseType(typeof(NurseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<NurseDTO>> GetNurseBySearch(CancellationToken cancellationToken , [FromQuery] SearchNuresQuery? query = null)
        {
                var nurses = await mediatR.Send(query, cancellationToken);
                if (nurses.Any())
                {
                    return Ok(nurses);
                }
                return NotFound();
            
        }


        [HttpGet("Profile")]
        [ProducesResponseType(typeof(PatientPeofileDTO),StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<PatientPeofileDTO>>> Profile(CancellationToken cancellationToken)
        {
            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var patient = await mediatR.Send(new GetPatientProfileQuery(patientId, cancellationToken)) ;
            if (patient.ISucsses)
            {
                return Ok(patient.Obj);
            }
            return NotFound();
        }

        [HttpGet("Appoinments")]
        [ProducesResponseType(typeof(AppointmentDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<List<AppointmentDTO>>>> Appoinments([FromQuery] int page,CancellationToken cancellationToken)
        {
           var patientId= User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var appoinment=await mediatR.Send(new GetAppointmentsQuery(patientId, page), cancellationToken);
            if (appoinment.ISucsses)
            {
                return Ok(appoinment.Obj);
            }
            return NotFound(appoinment);
        }

        [HttpGet("Reports")]
        [ProducesResponseType(typeof(AiReportDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<List<AiReportDTO>>>> Reports([FromQuery] int page, CancellationToken cancellationToken)
        {
            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var Report = await mediatR.Send(new GetAiReportsQuery(patientId,page), cancellationToken);
            if (Report.ISucsses)
            {
                return Ok(Report.Obj);
            }
            return NotFound(Report);
        }
        
        [HttpGet("Presciptions")]
        [ProducesResponseType(typeof(PresciptionDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse<List<PresciptionDTO>>>> Presciptions([FromQuery] int page, CancellationToken cancellationToken)
        {
            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var presciptions = await mediatR.Send(new GetPresciptionQuery(patientId,page), cancellationToken);
            if (presciptions.ISucsses)
            {
                return Ok(presciptions.Obj);
            }
            return NotFound(presciptions);
        }
        
        //[HttpGet("PresciptionItems")]
        //public async Task<ActionResult<PresciptionItemDTO>> PresciptionItems([FromQuery] GetPresciptionItemQuery query, CancellationToken cancellationToken)
        //{
        //    var Items = await mediatR.Send(query, cancellationToken);
        //    if (Items.Any())
        //    {
        //        return Ok(Items); 
        //    }
        //    return NotFound();
        //}


        [HttpGet("GetFullPrescriptionDetails/{id}")]
        [ProducesResponseType(typeof(PrescriptionFullDetailsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetFullPrescriptionDetails(string id) // غيرنا النوع هنا لـ string
        {
            // دلوقتى الـ id نوعه string والـ Query مستنية string
            var result = await mediatR.Send(new GetFullPrescriptionQuery(id));

            return result != null ? Ok(result) : NotFound("الروشتة غير موجودة");
        }



        [HttpPost("book-nurse")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]

        public async Task<ActionResult> BookNurse([FromQuery] string NurseId)
        {
            var pationtId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var pationt = await dbContext.Patients
                .FirstOrDefaultAsync(p => p.ID == pationtId);

            var booking = new Booking
            {
                PatientId = pationtId,
                NurseId = NurseId,
                Address = pationt.Address,
                City = pationt.City,
                Governorate = pationt.Governorate,
                Status = "Pending",
                RequestDate = DateTime.Now
            };

            dbContext.Bookings.Add(booking);
            await dbContext.SaveChangesAsync();

            return Ok(new { message = "تم انشاء الحجز بنجاح" });


        }
     
        [HttpGet("Get-patient-bookings-Nurse")]
        [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]

        public async Task<ActionResult> GetPatientBookings()
        {
            var pationtId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var bookings = await dbContext.Bookings
                .Where(b => b.PatientId == pationtId)
                .OrderByDescending(b => b.RequestDate)
                .ToListAsync();
            if (bookings is null || bookings.Count == 0)
            {
                return NotFound("لا يوجد حجوزات !");
            }
            return Ok(bookings);
        }

        [HttpGet("SearchForDrugInPharmacy")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchByDrugName([FromQuery] string drugName, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrEmpty(drugName))
                return BadRequest("ادخل اسم الدواء!");

            var result = await _pharmacyService.SearchByDrugNameAsync(drugName, pageNumber, pageSize);
            if (!result.Data.Any())
                return NotFound("مفيش صيدليات عندها الدواء ده!");

            return Ok(result);  
        }

        // ✅ أي حد يقدر يبحث بالكاتيجوري
        [HttpGet("SearchByDrugCategoryInPharmacy")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var result = await _pharmacyService.GetByCategoryAsync(category);
            if (!result.Any())
                return NotFound("مفيش صيدليات عندها أدوية في الكاتيجوري دي!");

            return Ok(result);
        }

        //[HttpPost("rating/{pharmacyId}")]
        //[Authorize(Roles = "Patient")]
        //public async Task<IActionResult> AddPharmacyRating(string pharmacyId, [FromQuery] string patientId, [FromQuery] int rating, [FromQuery] string? comment)
        //{
        //    if (rating < 1 || rating > 5)
        //        return BadRequest("التقييم لازم يكون من 1 لـ 5!");

        //    await _pharmacyService.AddRatingAsync(pharmacyId, patientId, rating, comment);
        //    return Ok("تم إضافة التقييم بنجاح!");
        //}

        [HttpPost("MakingRate")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<bool>> MakingRate( [FromQuery] string TargetId ,[FromQuery] StarsRatingEnum Rateing, [FromForm] string? Comment = null)
        {
            var PatientId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediatR.Send(
                new MakingCommentCommand(
                    Rateing, TargetId, PatientId, Comment));

            if (result.ISucsses)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);  
        }



        [HttpPost("MakingComment")]
        [Authorize(Roles = "Patient")]

        public async Task<ActionResult<bool>> MakingComment([FromForm] string Comment, [FromQuery] string TargetId)
        {
            var PatientId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediatR.Send(
                new CreateCommentCommand(
                      PatientId,TargetId, Comment));

            if (result.ISucsses)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);                                                      
        }

        [HttpGet("GetPatientOrders")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetPatientOrders()
        {
            var PatientId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await orderService.GetPatientOrdersAsync(PatientId);
            if (!result.Any())
                return NotFound("مفيش orders لهذا المريض!");

            return Ok(result);
        }
        [HttpGet("GetNearestPharmacies")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNearestPharmacies(
           [FromQuery] string? drugName,
           [FromQuery] double latitude,
           [FromQuery] double longitude,
           [FromQuery] double radius = 5)
        {
            if (string.IsNullOrEmpty(drugName))
                return BadRequest("ادخل اسم الدواء!");

            var result = await _pharmacyService.GetNearestPharmaciesAsync(drugName, latitude, longitude, radius);

            if (!result.Any())
                return NotFound("مفيش صيدليات قريبة عندها الدواء ده!");

            return Ok(result);
        }
    }
}

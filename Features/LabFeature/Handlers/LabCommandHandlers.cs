using AutoMapper;
using DataAccess;
using Features.LabFeature.Commands;
using Features.NotifecationService.Commands.CreateNotifcation;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using Models.Enums.NotificationEnums;
using Models.Models;
using Services.FileServices;
using System.Security.Claims;

namespace Features.LabFeature.Handlers
{
    public class LabCommandHandlers :
        IRequestHandler<UpdateLabProfileCommand, ResultResponse<string>>,
        IRequestHandler<UpdateLabWorkingHoursCommand, ResultResponse<string>>,
        IRequestHandler<UpdateLabLocationCommand, ResultResponse<string>>,
        IRequestHandler<UpdateLabLogoCommand, ResultResponse<string>>,
        IRequestHandler<AddLabTestCommand, ResultResponse<string>>,
        IRequestHandler<UpdateLabTestCommand, ResultResponse<string>>,
        IRequestHandler<DeleteLabTestCommand, ResultResponse<string>>,
        IRequestHandler<ChangeLabBookingStatusCommand, ResultResponse<string>>,
        IRequestHandler<UploadLabResultCommand, ResultResponse<string>>,
        IRequestHandler<DeleteLabResultCommand, ResultResponse<string>>,
        IRequestHandler<AssignCollectorCommand, ResultResponse<string>>,
        IRequestHandler<UpdateHomeCollectionStatusCommand, ResultResponse<string>>
    {
        private readonly IMediator mediator;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LabCommandHandlers(IMediator mediator,ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IFileService fileService, UserManager<ApplicationUser> userManager)
        {
            this.mediator = mediator;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
            _userManager = userManager;
        }

        private string? GetLabEmail() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        //public async Task<ResultResponse<string>> Handle(RegisterLabCommand request, CancellationToken cancellationToken)
        //{
        //    if (await _userManager.FindByEmailAsync(request.Email) != null)
        //        return new ResultResponse<string> { ISucsses = false, Message = "Email already exists" };

        //    var user = new ApplicationUser
        //    {
        //        UserName = request.Email,
        //        Email = request.Email,
        //        Address = request.Address,
        //        City = "Unknown", // Assuming AreaId resolves to City later
        //        Role = "Lab"
        //    };

        //    var result = await _userManager.CreateAsync(user, request.Password);
        //    if (!result.Succeeded)
        //        return new ResultResponse<string> { ISucsses = false, Message = "Failed to create user", Errors = result.Errors.Select(e => e.Description).ToList() };

        //    await _userManager.AddToRoleAsync(user, "Lab");

        //    var lab = new Lab
        //    {
        //        Name = request.Name,
        //        Email = request.Email,
        //        Address = request.Address,
        //        Phone = request.Phone,
        //        AreaId = request.AreaId,
        //        LabLicense = request.LabLicense,
        //        Status = ConfrmationStatus.Pending,
        //        IsActive = false
        //    };

        //    _context.Labs.Add(lab);
        //    await _context.SaveChangesAsync(cancellationToken);

        //    return new ResultResponse<string> { ISucsses = true, Message = "Lab registered successfully and pending admin approval.", Obj = user.Id };
        //}

        public async Task<ResultResponse<string>> Handle(UpdateLabProfileCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            lab.Name = request.Name;
            lab.Phone = request.Phone;
            lab.Address = request.Address;
            lab.AreaId = request.AreaId;

            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Profile updated successfully" };
        }

        public async Task<ResultResponse<string>> Handle(UpdateLabWorkingHoursCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            lab.WorkingHours = request.WorkingHours;
            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Working hours updated successfully" };
        }

        public async Task<ResultResponse<string>> Handle(UpdateLabLocationCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            lab.Latitude = request.Latitude;
            lab.Longitude = request.Longitude;
            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Location updated successfully" };
        }

        public async Task<ResultResponse<string>> Handle(UpdateLabLogoCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            var fileName = await _fileService.UploadFileAsync(request.File, "uploads/labs", cancellationToken);
            lab.ImageUrl = "/uploads/labs/" + fileName;

            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Logo updated successfully", Obj = lab.ImageUrl };
        }

        // --- Tests CRUD ---
        public async Task<ResultResponse<string>> Handle(AddLabTestCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            var testOffer = new LabTestOffer
            {
                LabId = lab.ID,
                Price = request.Price,
                // Wait, MedicalTest vs LabTestOffer. We need to create the MedicalTest if it doesn't exist?
                // The architecture seems to have MedicalTest as a central dictionary and LabTestOffer as the lab's pricing for it.
                // But the user requested "POST /api/Lab/Tests" with Name, Category, Price, etc.
                // We'll create a MedicalTest and attach it as LabTestOffer for this lab.
            };

            var medicalTest = new MedicalTest
            {
                Name = request.Name,
                Category = request.Category,
                TurnaroundHours = int.TryParse(request.EstimatedTime, out var t1) ? t1 : 24,
                Description = request.Description
            };
            
            _context.MedicalTests.Add(medicalTest);
            testOffer.MedicalTestId = medicalTest.ID;
            _context.LabTestOffers.Add(testOffer);
            
            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Test added successfully", Obj = medicalTest.ID };
        }

        public async Task<ResultResponse<string>> Handle(UpdateLabTestCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            var testOffer = await _context.LabTestOffers
                .Include(t => t.MedicalTest)
                .FirstOrDefaultAsync(t => t.MedicalTestId == request.Id && t.LabId == lab.ID && !t.IsDeleted, cancellationToken);
            
            if (testOffer == null) return new ResultResponse<string> { ISucsses = false, Message = "Test not found in your lab" };

            testOffer.Price = request.Price;
            if(testOffer.MedicalTest != null)
            {
                testOffer.MedicalTest.Name = request.Name;
                testOffer.MedicalTest.Category = request.Category;
                testOffer.MedicalTest.TurnaroundHours = int.TryParse(request.EstimatedTime, out var t2) ? t2 : 24;
                testOffer.MedicalTest.Description = request.Description;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Test updated successfully" };
        }

        public async Task<ResultResponse<string>> Handle(DeleteLabTestCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            var testOffer = await _context.LabTestOffers
                .FirstOrDefaultAsync(t => t.MedicalTestId == request.Id && t.LabId == lab.ID && !t.IsDeleted, cancellationToken);
            
            if (testOffer == null) return new ResultResponse<string> { ISucsses = false, Message = "Test not found in your lab" };

            testOffer.IsDeleted = true;
            testOffer.DeletedAT = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Test deleted successfully" };
        }

        // --- Booking Workflow ---
        public async Task<ResultResponse<string>> Handle(ChangeLabBookingStatusCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            var booking = await _context.LabBookings
                .FirstOrDefaultAsync(b => b.ID == request.Id && b.LabId == lab.ID && !b.IsDeleted, cancellationToken);

            if (booking == null) return new ResultResponse<string> { ISucsses = false, Message = "Booking not found" };

            booking.Status = request.Status;
            await _context.SaveChangesAsync(cancellationToken);
            
         
            await _context.SaveChangesAsync(cancellationToken);
            await mediator.Send(new CreateNotificationCommand(
                       ReceiverId: booking.PatientId,
                       SenderId: booking.LabId,
                       Title: "booking Lab",
                       Body: $"Your lab booking status was updated to {request.Status}",
                       Type: NotificationTypeEnum.LabCoolectorAssign,
                       ReferenceType: NotificationReferenceType.LabResult,
                       ReferenceId: booking.ID
                    ), cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Message = $"Status updated to {request.Status}" };
        }

        public async Task<ResultResponse<string>> Handle(AssignCollectorCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            var booking = await _context.LabBookings.FirstOrDefaultAsync(b => b.ID == request.Id && b.LabId == lab.ID && !b.IsDeleted, cancellationToken);
            if (booking == null) return new ResultResponse<string> { ISucsses = false, Message = "Booking not found" };

            if (booking.VisitType != VisitTypeEnum.HomeCollection)
                return new ResultResponse<string> { ISucsses = false, Message = "This booking is not a Home Collection request." };

            if (booking.Status != LabBookingStatusEnum.Accepted)
                return new ResultResponse<string> { ISucsses = false, Message = "Booking must be Accepted before assigning a collector." };

            booking.CollectorId = request.CollectorId;
            booking.Status = LabBookingStatusEnum.CollectorAssigned;

            await _context.SaveChangesAsync(cancellationToken);
            await mediator.Send(new CreateNotificationCommand(
               ReceiverId: booking.PatientId,
               SenderId: booking.LabId,
               Title: "booking Lab",
               Body: "تم تعيين مندوب لسحب العينة وسيتواصل معك قريباً.",
               Type: NotificationTypeEnum.LabCoolectorAssign,
               ReferenceType: NotificationReferenceType.LabResult,
               ReferenceId: booking.ID
           ), cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Collector assigned successfully." };
        }

        public async Task<ResultResponse<string>> Handle(UpdateHomeCollectionStatusCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            var booking = await _context.LabBookings.FirstOrDefaultAsync(b => b.ID == request.Id && b.LabId == lab.ID && !b.IsDeleted, cancellationToken);
            if (booking == null) return new ResultResponse<string> { ISucsses = false, Message = "Booking not found" };

            if (booking.VisitType != VisitTypeEnum.HomeCollection)
                return new ResultResponse<string> { ISucsses = false, Message = "This booking is not a Home Collection request." };

            if (request.NewStatus == LabBookingStatusEnum.SampleCollected && string.IsNullOrEmpty(booking.CollectorId))
                return new ResultResponse<string> { ISucsses = false, Message = "Cannot mark as collected without assigning a collector first." };

            if (request.NewStatus == LabBookingStatusEnum.Completed)
            {
                var hasResults = await _context.LabTestResults.AnyAsync(r => r.LabBookingItem.LabBookingId == booking.ID && !r.IsDeleted, cancellationToken);
                if (!hasResults) return new ResultResponse<string> { ISucsses = false, Message = "Cannot complete booking without uploading results first." };
            }

            booking.Status = request.NewStatus;

            string notifyMessage = request.NewStatus switch
            {
                LabBookingStatusEnum.Accepted => "تم قبول طلب السحب المنزلي الخاص بك.",
                LabBookingStatusEnum.CollectorStartedTrip => "المندوب في الطريق إليك الآن.",
                LabBookingStatusEnum.SampleCollected => "تم سحب العينة بنجاح.",
                LabBookingStatusEnum.ReturnedToLab => "وصلت العينة إلى المعمل وجاري فحصها.",
                LabBookingStatusEnum.Completed => "نتيجة التحليل أصبحت جاهزة.",
                LabBookingStatusEnum.Rejected => "عذراً، تم رفض طلب السحب المنزلي الخاص بك.",
                _ => $"تم تحديث حالة الطلب إلى {request.NewStatus}"
            };

           

            await _context.SaveChangesAsync(cancellationToken);

            await mediator.Send(new CreateNotificationCommand(
              ReceiverId: booking.PatientId,
              SenderId: booking.LabId,
              Title: "booking Lab",
              Body: notifyMessage,
              Type: NotificationTypeEnum.LabCoolectorAssign,
              ReferenceType: NotificationReferenceType.LabResult,
              ReferenceId: booking.ID
          ), cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Message = $"Status successfully updated to {request.NewStatus}." };
        }

        // --- Results ---
        public async Task<ResultResponse<string>> Handle(UploadLabResultCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            var booking = await _context.LabBookings
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.ID == request.Id && b.LabId == lab.ID && !b.IsDeleted, cancellationToken);

            if (booking == null) return new ResultResponse<string> { ISucsses = false, Message = "Booking not found" };

            // We apply result to all items or just one? Usually lab returns one PDF for the whole booking.
            // We'll attach it to the first item for simplicity, or create a LabTestResult if none exist.
            var item = booking.Items.FirstOrDefault();
            if (item == null) return new ResultResponse<string> { ISucsses = false, Message = "Booking has no items" };

            var uploadedFiles = new List<string>();
            foreach(var file in request.Files)
            {
                var fileName = await _fileService.UploadFileAsync(file, "uploads/results", cancellationToken);
                uploadedFiles.Add("/uploads/results/" + fileName);
            }
            string filesStr = string.Join(",", uploadedFiles);
            
            var result = await _context.LabTestResults.FirstOrDefaultAsync(r => r.LabBookingItemId == item.ID, cancellationToken);
            if (result == null)
            {
                result = new LabTestResult
                {
                    LabBookingItemId = item.ID,
                    Status = ResultStatusEnum.Ready,
                    ResultFileUrl = filesStr,
                    ResultValuesJson = request.JsonResult,
                    DoctorNotes = request.DoctorNotes,
                    ReportedAt = DateTime.Now
                };
                _context.LabTestResults.Add(result);
            }
            else
            {
                result.ResultFileUrl = filesStr;
                result.ResultValuesJson = request.JsonResult ?? result.ResultValuesJson;
                result.DoctorNotes = request.DoctorNotes ?? result.DoctorNotes;
                result.Status = ResultStatusEnum.Ready;
                result.ReportedAt = DateTime.Now;
            }

            booking.Status = LabBookingStatusEnum.Completed; // Automatically complete
            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Message = "Result uploaded successfully" };
        }

        public async Task<ResultResponse<string>> Handle(DeleteLabResultCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            var booking = await _context.LabBookings
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.ID == request.Id && b.LabId == lab.ID && !b.IsDeleted, cancellationToken);

            if (booking == null) return new ResultResponse<string> { ISucsses = false, Message = "Booking not found" };

            var item = booking.Items.FirstOrDefault();
            if (item != null)
            {
                var result = await _context.LabTestResults.FirstOrDefaultAsync(r => r.LabBookingItemId == item.ID && !r.IsDeleted, cancellationToken);
                if (result != null)
                {
                    result.IsDeleted = true;
                    result.DeletedAT = DateTime.Now;
                    await _context.SaveChangesAsync(cancellationToken);
                    return new ResultResponse<string> { ISucsses = true, Message = "Result deleted successfully" };
                }
            }

            return new ResultResponse<string> { ISucsses = false, Message = "No result found for this booking" };
        }
    }
}

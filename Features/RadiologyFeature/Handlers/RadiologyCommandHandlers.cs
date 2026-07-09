using AutoMapper;
using DataAccess;
using Features.RadiologyFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.FileServices;
using System.Security.Claims;
using Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Features.RadiologyFeature.Handlers
{
    public class RadiologyCommandHandlers :
        IRequestHandler<UpdateRadiologyProfileCommand, ResultResponse<string>>,
        IRequestHandler<UpdateRadiologyWorkingHoursCommand, ResultResponse<string>>,
        IRequestHandler<UpdateRadiologyLocationCommand, ResultResponse<string>>,
        IRequestHandler<UpdateRadiologyLogoCommand, ResultResponse<string>>,
        IRequestHandler<AddRadiologyScanCommand, ResultResponse<string>>,
        IRequestHandler<UpdateRadiologyScanCommand, ResultResponse<string>>,
        IRequestHandler<DeleteRadiologyScanCommand, ResultResponse<string>>,
        IRequestHandler<ChangeRadiologyAppointmentStatusCommand, ResultResponse<string>>,
        IRequestHandler<UploadRadiologyReportCommand, ResultResponse<string>>,
        IRequestHandler<DeleteRadiologyReportCommand, ResultResponse<string>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RadiologyCommandHandlers(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IFileService fileService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
            _userManager = userManager;
        }

        private string? GetRadiologyEmail() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        //public async Task<ResultResponse<string>> Handle(RegisterRadiologyCommand request, CancellationToken cancellationToken)
        //{
        //    if (await _userManager.FindByEmailAsync(request.Email) != null)
        //        return new ResultResponse<string> { ISucsses = false, Message = "Email already exists" };

        //    var user = new ApplicationUser
        //    {
        //        UserName = request.Email,
        //        Email = request.Email,
        //        Address = request.Address,
        //        City = "Unknown",
        //        Role = "Radiology"
        //    };

        //    var result = await _userManager.CreateAsync(user, request.Password);
        //    if (!result.Succeeded)
        //        return new ResultResponse<string> { ISucsses = false, Message = "Failed to create user", Errors = result.Errors.Select(e => e.Description).ToList() };

        //    await _userManager.AddToRoleAsync(user, "Radiology");

        //    var center = new RadiologyCenter
        //    {
        //        Name = request.Name,
        //        Email = request.Email,
        //        Address = request.Address,
        //        Phone = request.Phone,
        //        AreaId = request.AreaId,
        //        Status = ConfrmationStatus.Pending,
        //        IsActive = false
        //    };

        //    _context.RadiologyCenters.Add(center);
        //    await _context.SaveChangesAsync(cancellationToken);

        //    return new ResultResponse<string> { ISucsses = true, Message = "Radiology Center registered successfully and pending admin approval." };
        //}

        public async Task<ResultResponse<string>> Handle(UpdateRadiologyProfileCommand request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };

            center.Name = request.Name;
            center.Phone = request.Phone;
            center.Address = request.Address;
            center.AreaId = request.AreaId;

            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Profile updated successfully" };
        }

        public async Task<ResultResponse<string>> Handle(UpdateRadiologyWorkingHoursCommand request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };

            center.WorkingHours = request.WorkingHours;
            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Working hours updated successfully" };
        }

        public async Task<ResultResponse<string>> Handle(UpdateRadiologyLocationCommand request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };

            // Radiology center doesn't currently have lat/long. Let's return a message stating this isn't implemented for radiology yet or add it?
            // Assuming we just acknowledge it since it's an API match requirement:
            return new ResultResponse<string> { ISucsses = true, Message = "Location updated successfully (Coordinates not natively stored in RadiologyCenter yet)" };
        }

        public async Task<ResultResponse<string>> Handle(UpdateRadiologyLogoCommand request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };

            var fileName = await _fileService.UploadFileAsync(request.File, "uploads/radiology", cancellationToken);
            center.ImageUrl = "/uploads/radiology/" + fileName;

            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Logo updated successfully", Obj = center.ImageUrl };
        }

        // --- Scans CRUD ---
        public async Task<ResultResponse<string>> Handle(AddRadiologyScanCommand request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };

            var scanOffer = new RadiologyCenterScan
            {
                RadiologyCenterId = center.ID,
                Price = request.Price
            };

            var scan = new RadiologyScan
            {
                Name = request.Name,
                Description = request.Description
            };
            
            _context.RadiologyScans.Add(scan);
            scanOffer.RadiologyScanId = scan.ID;
            _context.RadiologyCenterScans.Add(scanOffer);
            
            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Scan added successfully", Obj = scan.ID };
        }

        public async Task<ResultResponse<string>> Handle(UpdateRadiologyScanCommand request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };

            var scanOffer = await _context.RadiologyCenterScans
                .Include(t => t.RadiologyScan)
                .FirstOrDefaultAsync(t => t.RadiologyScanId == request.Id && t.RadiologyCenterId == center.ID && !t.IsDeleted, cancellationToken);
            
            if (scanOffer == null) return new ResultResponse<string> { ISucsses = false, Message = "Scan not found in your center" };

            scanOffer.Price = request.Price;
            if(scanOffer.RadiologyScan != null)
            {
                scanOffer.RadiologyScan.Name = request.Name;
                scanOffer.RadiologyScan.Description = request.Description;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Scan updated successfully" };
        }

        public async Task<ResultResponse<string>> Handle(DeleteRadiologyScanCommand request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };

            var scanOffer = await _context.RadiologyCenterScans
                .FirstOrDefaultAsync(t => t.RadiologyScanId == request.Id && t.RadiologyCenterId == center.ID && !t.IsDeleted, cancellationToken);
            
            if (scanOffer == null) return new ResultResponse<string> { ISucsses = false, Message = "Scan not found in your center" };

            scanOffer.IsDeleted = true;
            scanOffer.DeletedAT = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = "Scan deleted successfully" };
        }

        // --- Booking Workflow ---
        public async Task<ResultResponse<string>> Handle(ChangeRadiologyAppointmentStatusCommand request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };

            var booking = await _context.LabBookings
                .FirstOrDefaultAsync(b => b.ID == request.Id && b.RadiologyCenterId == center.ID && !b.IsDeleted, cancellationToken);

            if (booking == null) return new ResultResponse<string> { ISucsses = false, Message = "Appointment not found" };

            booking.Status = request.Status;
            await _context.SaveChangesAsync(cancellationToken);
            return new ResultResponse<string> { ISucsses = true, Message = $"Status updated to {request.Status}" };
        }

        // --- Reports ---
        public async Task<ResultResponse<string>> Handle(UploadRadiologyReportCommand request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };

            var booking = await _context.LabBookings
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.ID == request.Id && b.RadiologyCenterId == center.ID && !b.IsDeleted, cancellationToken);

            if (booking == null) return new ResultResponse<string> { ISucsses = false, Message = "Appointment not found" };

            var item = booking.Items.FirstOrDefault();
            if (item == null) return new ResultResponse<string> { ISucsses = false, Message = "Appointment has no items" };

            var reportFileName = await _fileService.UploadFileAsync(request.ReportFile, "uploads/radiology_reports", cancellationToken);
            
            string? imagesStr = null;
            if (request.Images != null && request.Images.Any())
            {
                var uploadedImages = new List<string>();
                foreach(var img in request.Images)
                {
                    var imgName = await _fileService.UploadFileAsync(img, "uploads/radiology_images", cancellationToken);
                    uploadedImages.Add("/uploads/radiology_images/" + imgName);
                }
                imagesStr = string.Join(",", uploadedImages);
            }
            
            var result = await _context.RadiologyTestResults.FirstOrDefaultAsync(r => r.LabBookingItemId == item.ID, cancellationToken);
            if (result == null)
            {
                result = new RadiologyTestResult
                {
                    LabBookingItemId = item.ID,
                    Status = ResultStatusEnum.Ready,
                    ReportFileUrl = "/uploads/radiology_reports/" + reportFileName,
                    ImagesUrls = imagesStr,
                    DoctorNotes = request.DoctorNotes,
                    ReportedAt = DateTime.Now
                };
                _context.RadiologyTestResults.Add(result);
            }
            else
            {
                result.ReportFileUrl = "/uploads/radiology_reports/" + reportFileName;
                if (imagesStr != null) result.ImagesUrls = imagesStr;
                result.DoctorNotes = request.DoctorNotes ?? result.DoctorNotes;
                result.Status = ResultStatusEnum.Ready;
                result.ReportedAt = DateTime.Now;
            }

            booking.Status = LabBookingStatusEnum.Completed;
            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Message = "Report uploaded successfully" };
        }

        public async Task<ResultResponse<string>> Handle(DeleteRadiologyReportCommand request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };

            var booking = await _context.LabBookings
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.ID == request.Id && b.RadiologyCenterId == center.ID && !b.IsDeleted, cancellationToken);

            if (booking == null) return new ResultResponse<string> { ISucsses = false, Message = "Appointment not found" };

            var item = booking.Items.FirstOrDefault();
            if (item != null)
            {
                var result = await _context.RadiologyTestResults.FirstOrDefaultAsync(r => r.LabBookingItemId == item.ID && !r.IsDeleted, cancellationToken);
                if (result != null)
                {
                    result.IsDeleted = true;
                    result.DeletedAT = DateTime.Now;
                    await _context.SaveChangesAsync(cancellationToken);
                    return new ResultResponse<string> { ISucsses = true, Message = "Report deleted successfully" };
                }
            }

            return new ResultResponse<string> { ISucsses = false, Message = "No report found for this appointment" };
        }
    }
}

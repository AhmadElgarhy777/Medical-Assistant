using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Features.RadiologyFeature.Commands;
using Features.RadiologyFeature.Queries;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RadiologyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RadiologyController(IMediator mediator)
        {
            _mediator = mediator;
        }

       

        [Authorize(Roles = "Radiology")]
        [HttpGet("Dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _mediator.Send(new GetRadiologyDashboardQuery());
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _mediator.Send(new GetRadiologyProfileQuery());
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateRadiologyProfileCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpPut("WorkingHours")]
        public async Task<IActionResult> UpdateWorkingHours([FromBody] UpdateRadiologyWorkingHoursCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpPut("Location")]
        public async Task<IActionResult> UpdateLocation([FromBody] UpdateRadiologyLocationCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpPut("Logo")]
        public async Task<IActionResult> UpdateLogo(IFormFile file)
        {
            var result = await _mediator.Send(new UpdateRadiologyLogoCommand(file));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        // Scans Management
        [Authorize(Roles = "Radiology")]
        [HttpGet("Scans")]
        public async Task<IActionResult> GetScans([FromQuery] PaginationRequest request)
        {
            var result = await _mediator.Send(new GetRadiologyScansQuery(request));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpPost("Scans")]
        public async Task<IActionResult> AddScan([FromBody] AddRadiologyScanCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpPut("Scans/{id}")]
        public async Task<IActionResult> UpdateScan(string id, [FromBody] UpdateRadiologyScanCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpDelete("Scans/{id}")]
        public async Task<IActionResult> DeleteScan(string id)
        {
            var result = await _mediator.Send(new DeleteRadiologyScanCommand(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        // Appointments Management
        [Authorize(Roles = "Radiology")]
        [HttpGet("Appointments")]
        public async Task<IActionResult> GetAppointments([FromQuery] PaginationRequest request, [FromQuery] string? status)
        {
            var result = await _mediator.Send(new GetRadiologyAppointmentsQuery(request, status));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpGet("Appointments/{id}")]
        public async Task<IActionResult> GetAppointmentDetails(string id)
        {
            var result = await _mediator.Send(new GetRadiologyAppointmentDetailsQuery(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        // Appointment Workflow
        [Authorize(Roles = "Radiology")]
        [HttpPut("Appointments/{id}/Accept")]
        public async Task<IActionResult> AcceptAppointment(string id) => await HandleStatusChange(id, Models.Enums.LabBookingStatusEnum.Accepted);

        [Authorize(Roles = "Radiology")]
        [HttpPut("Appointments/{id}/Reject")]
        public async Task<IActionResult> RejectAppointment(string id) => await HandleStatusChange(id, Models.Enums.LabBookingStatusEnum.Rejected);

        [Authorize(Roles = "Radiology")]
        [HttpPut("Appointments/{id}/StartProcessing")]
        public async Task<IActionResult> StartProcessing(string id) => await HandleStatusChange(id, Models.Enums.LabBookingStatusEnum.Processing);

        [Authorize(Roles = "Radiology")]
        [HttpPut("Appointments/{id}/Complete")]
        public async Task<IActionResult> CompleteAppointment(string id) => await HandleStatusChange(id, Models.Enums.LabBookingStatusEnum.Completed);

        private async Task<IActionResult> HandleStatusChange(string id, Models.Enums.LabBookingStatusEnum status)
        {
            var result = await _mediator.Send(new ChangeRadiologyAppointmentStatusCommand(id, status));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        // Reports
        [Authorize(Roles = "Radiology")]
        [HttpPost("Appointments/{id}/UploadReport")]
        public async Task<IActionResult> UploadReport(string id, IFormFile reportFile, [FromForm] List<IFormFile>? images, [FromForm] string? doctorNotes)
        {
            var result = await _mediator.Send(new UploadRadiologyReportCommand(id, reportFile, images, doctorNotes));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpGet("Appointments/{id}/Report")]
        public async Task<IActionResult> GetReport(string id)
        {
            var result = await _mediator.Send(new GetRadiologyReportQuery(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpDelete("Appointments/{id}/Report")]
        public async Task<IActionResult> DeleteReport(string id)
        {
            var result = await _mediator.Send(new DeleteRadiologyReportCommand(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        // Schedule
        [Authorize(Roles = "Radiology")]
        [HttpGet("Schedule")]
        public async Task<IActionResult> GetSchedule()
        {
            var result = await _mediator.Send(new GetRadiologyScheduleQuery());
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpPost("Schedule")]
        public async Task<IActionResult> AddSchedule([FromBody] AddRadiologyScheduleCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpPut("Schedule/{id}")]
        public async Task<IActionResult> UpdateSchedule(string id, [FromBody] UpdateRadiologyScheduleCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpDelete("Schedule/{id}")]
        public async Task<IActionResult> DeleteSchedule(string id)
        {
            var result = await _mediator.Send(new DeleteRadiologyScheduleCommand(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

       

        // Analytics
        [Authorize(Roles = "Radiology")]
        [HttpGet("Analytics/Daily")]
        public async Task<IActionResult> GetDailyAnalytics()
        {
            var result = await _mediator.Send(new GetRadiologyAnalyticsQuery("Daily"));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpGet("Analytics/Weekly")]
        public async Task<IActionResult> GetWeeklyAnalytics()
        {
            var result = await _mediator.Send(new GetRadiologyAnalyticsQuery("Weekly"));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Radiology")]
        [HttpGet("Analytics/Monthly")]
        public async Task<IActionResult> GetMonthlyAnalytics()
        {
            var result = await _mediator.Send(new GetRadiologyAnalyticsQuery("Monthly"));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        // --- Patient APIs for Radiology ---
        [Authorize(Roles = "Patient")]
        [HttpGet("Areas")]
        public async Task<IActionResult> GetAreas()
        {
            var result = await _mediator.Send(new GetRadiologyAreasQuery());
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("ByArea")]
        public async Task<IActionResult> GetCentersByArea([FromQuery] string areaId, [FromQuery] PaginationRequest request)
        {
            var result = await _mediator.Send(new GetRadiologyByAreaQuery(areaId, request));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetCenterDetails(string id)
        {
            var result = await _mediator.Send(new GetRadiologyDetailsForPatientQuery(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpPost("BookNow")]
        public async Task<IActionResult> BookNow([FromBody] BookRadiologyScanCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("MyBookings")]
        public async Task<IActionResult> GetMyBookings([FromQuery] PaginationRequest request)
        {
            var result = await _mediator.Send(new GetPatientRadiologyBookingsQuery(request));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("MyBookings/{id}")]
        public async Task<IActionResult> GetMyBookingDetails(string id)
        {
            var result = await _mediator.Send(new GetPatientRadiologyBookingDetailsQuery(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpPut("CancelBooking/{id}")]
        public async Task<IActionResult> CancelBooking(string id)
        {
            var result = await _mediator.Send(new CancelPatientRadiologyBookingCommand(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }
    }
}
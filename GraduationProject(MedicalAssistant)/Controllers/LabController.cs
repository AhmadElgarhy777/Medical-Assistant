using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Features.LabFeature.Commands;
using Features.LabFeature.Queries;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LabController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpPost("Register")]
        //public async Task<IActionResult> Register([FromBody] RegisterLabCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    return result.ISucsses ? Ok(result) : BadRequest(result);
        //}

        [Authorize(Roles = "Lab")]
        [HttpGet("Dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _mediator.Send(new GetLabDashboardQuery());
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _mediator.Send(new GetLabProfileQuery());
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateLabProfileCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPut("WorkingHours")]
        public async Task<IActionResult> UpdateWorkingHours([FromBody] UpdateLabWorkingHoursCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPut("Location")]
        public async Task<IActionResult> UpdateLocation([FromBody] UpdateLabLocationCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPut("Logo")]
        public async Task<IActionResult> UpdateLogo(IFormFile file)
        {
            var result = await _mediator.Send(new UpdateLabLogoCommand(file));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        // Tests Management
        [Authorize(Roles = "Lab")]
        [HttpGet("Tests")]
        public async Task<IActionResult> GetTests([FromQuery] PaginationRequest request)
        {
            var result = await _mediator.Send(new GetLabTestsQuery(request));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPost("Tests")]
        public async Task<IActionResult> AddTest([FromBody] AddLabTestCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPut("Tests/{id}")]
        public async Task<IActionResult> UpdateTest(string id, [FromBody] UpdateLabTestCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpDelete("Tests/{id}")]
        public async Task<IActionResult> DeleteTest(string id)
        {
            var result = await _mediator.Send(new DeleteLabTestCommand(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        // Booking Management
        [Authorize(Roles = "Lab")]
        [HttpGet("Bookings")]
        public async Task<IActionResult> GetBookings([FromQuery] PaginationRequest request, [FromQuery] string? status)
        {
            var result = await _mediator.Send(new GetLabBookingsQuery(request, status));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpGet("Bookings/{id}")]
        public async Task<IActionResult> GetBookingDetails(string id)
        {
            var result = await _mediator.Send(new GetLabBookingDetailsQuery(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        // Booking Workflow
        [Authorize(Roles = "Lab")]
        [HttpPut("Bookings/{id}/Accept")]
        public async Task<IActionResult> AcceptBooking(string id) => await HandleStatusChange(id, Models.Enums.LabBookingStatusEnum.Accepted);

        [Authorize(Roles = "Lab")]
        [HttpPut("Bookings/{id}/Reject")]
        public async Task<IActionResult> RejectBooking(string id) => await HandleStatusChange(id, Models.Enums.LabBookingStatusEnum.Rejected);

        [Authorize(Roles = "Lab")]
        [HttpPut("Bookings/{id}/StartProcessing")]
        public async Task<IActionResult> StartProcessing(string id) => await HandleStatusChange(id, Models.Enums.LabBookingStatusEnum.Processing);

        [Authorize(Roles = "Lab")]
        [HttpPut("Bookings/{id}/Complete")]
        public async Task<IActionResult> CompleteBooking(string id) => await HandleStatusChange(id, Models.Enums.LabBookingStatusEnum.Completed);

        private async Task<IActionResult> HandleStatusChange(string id, Models.Enums.LabBookingStatusEnum status)
        {
            var result = await _mediator.Send(new ChangeLabBookingStatusCommand(id, status));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        // Home Collection Workflow
        [Authorize(Roles = "Lab")]
        [HttpGet("HomeCollectionRequests")]
        public async Task<IActionResult> GetHomeCollectionRequests([FromQuery] PaginationRequest request, [FromQuery] string? status)
        {
            var result = await _mediator.Send(new GetHomeCollectionRequestsQuery(request, status));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpGet("HomeCollectionRequests/{id}")]
        public async Task<IActionResult> GetHomeCollectionRequestDetails(string id)
        {
            var result = await _mediator.Send(new GetHomeCollectionRequestDetailsQuery(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPut("HomeCollectionRequests/{id}/Accept")]
        public async Task<IActionResult> AcceptHomeCollection(string id)
        {
            var result = await _mediator.Send(new UpdateHomeCollectionStatusCommand(id, Models.Enums.LabBookingStatusEnum.Accepted));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPut("HomeCollectionRequests/{id}/Reject")]
        public async Task<IActionResult> RejectHomeCollection(string id)
        {
            var result = await _mediator.Send(new UpdateHomeCollectionStatusCommand(id, Models.Enums.LabBookingStatusEnum.Rejected));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPut("HomeCollectionRequests/{id}/AssignCollector")]
        public async Task<IActionResult> AssignCollectorToHomeRequest(string id, [FromBody] string collectorId)
        {
            var result = await _mediator.Send(new AssignCollectorCommand(id, collectorId));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPut("HomeCollectionRequests/{id}/CollectorStarted")]
        public async Task<IActionResult> CollectorStartedTrip(string id)
        {
            var result = await _mediator.Send(new UpdateHomeCollectionStatusCommand(id, Models.Enums.LabBookingStatusEnum.CollectorStartedTrip));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPut("HomeCollectionRequests/{id}/SampleCollected")]
        public async Task<IActionResult> SampleCollectedFromHome(string id)
        {
            var result = await _mediator.Send(new UpdateHomeCollectionStatusCommand(id, Models.Enums.LabBookingStatusEnum.SampleCollected));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPut("HomeCollectionRequests/{id}/ReturnedToLab")]
        public async Task<IActionResult> ReturnedToLab(string id)
        {
            var result = await _mediator.Send(new UpdateHomeCollectionStatusCommand(id, Models.Enums.LabBookingStatusEnum.ReturnedToLab));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        // Results
        [Authorize(Roles = "Lab")]
        [HttpPost("Bookings/{id}/UploadResult")]
        public async Task<IActionResult> UploadResult(string id, [FromForm] List<IFormFile> files, [FromForm] string? jsonResult, [FromForm] string? doctorNotes)
        {
            var result = await _mediator.Send(new UploadLabResultCommand(id, files, jsonResult, doctorNotes));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpGet("Bookings/{id}/Result")]
        public async Task<IActionResult> GetResult(string id)
        {
            var result = await _mediator.Send(new GetLabResultQuery(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpDelete("Bookings/{id}/Result")]
        public async Task<IActionResult> DeleteResult(string id)
        {
            var result = await _mediator.Send(new DeleteLabResultCommand(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        // Schedule
        [Authorize(Roles = "Lab")]
        [HttpGet("Schedule")]
        public async Task<IActionResult> GetSchedule()
        {
            var result = await _mediator.Send(new GetLabScheduleQuery());
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPost("Schedule")]
        public async Task<IActionResult> AddSchedule([FromBody] AddLabScheduleCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpPut("Schedule/{id}")]
        public async Task<IActionResult> UpdateSchedule(string id, [FromBody] UpdateLabScheduleCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpDelete("Schedule/{id}")]
        public async Task<IActionResult> DeleteSchedule(string id)
        {
            var result = await _mediator.Send(new DeleteLabScheduleCommand(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

       

        // Reports
        [Authorize(Roles = "Lab")]
        [HttpGet("Reports/Daily")]
        public async Task<IActionResult> GetDailyReport()
        {
            var result = await _mediator.Send(new GetLabReportQuery("Daily"));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpGet("Reports/Weekly")]
        public async Task<IActionResult> GetWeeklyReport()
        {
            var result = await _mediator.Send(new GetLabReportQuery("Weekly"));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Lab")]
        [HttpGet("Reports/Monthly")]
        public async Task<IActionResult> GetMonthlyReport()
        {
            var result = await _mediator.Send(new GetLabReportQuery("Monthly"));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        // --- Patient APIs for Lab ---
        [Authorize(Roles = "Patient")]
        [HttpGet("Areas")]
        public async Task<IActionResult> GetAreas()
        {
            var result = await _mediator.Send(new GetLabAreasQuery());
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("ByArea")]
        public async Task<IActionResult> GetLabsByArea([FromQuery] string areaId, [FromQuery] PaginationRequest request)
        {
            var result = await _mediator.Send(new GetLabsByAreaQuery(areaId, request));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetLabDetails(string id)
        {
            var result = await _mediator.Send(new GetLabDetailsForPatientQuery(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("SearchTests")]
        public async Task<IActionResult> SearchTests([FromQuery] string query, [FromQuery] PaginationRequest request)
        {
            var result = await _mediator.Send(new SearchLabTestsQuery(query, request));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpPost("BookNow")]
        public async Task<IActionResult> BookNow([FromBody] BookLabTestCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("MyBookings")]
        public async Task<IActionResult> GetMyBookings([FromQuery] PaginationRequest request)
        {
            var result = await _mediator.Send(new GetPatientLabBookingsQuery(request));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("MyBookings/{id}")]
        public async Task<IActionResult> GetMyBookingDetails(string id)
        {
            var result = await _mediator.Send(new GetPatientLabBookingDetailsQuery(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpPut("CancelBooking/{id}")]
        public async Task<IActionResult> CancelBooking(string id)
        {
            var result = await _mediator.Send(new CancelPatientLabBookingCommand(id));
            return result.ISucsses ? Ok(result) : BadRequest(result);
        }
    }
}
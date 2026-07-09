using Features.PatientScanRequet.Commands.AnalyzeRequestedScanOrcastraor;
using Features.PatientScanRequet.Commands.ApproveUploadedScan;
using Features.PatientScanRequet.Commands.CancelScanRequest;
using Features.PatientScanRequet.Commands.CreateScanRequest;
using Features.PatientScanRequet.Commands.PublishAiReport;
using Features.PatientScanRequet.Commands.RejectUploadedScan;
using Features.PatientScanRequet.Commands.UploadRequestedScan;
using Features.PatientScanRequet.Queries.GetAllDoctorScanRequests;
using Features.PatientScanRequet.Queries.GetScanRequestDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Enums;
using Utility;

namespace GraduationProject_MedicalAssistant_.Controllers
{

    public class PatientScanRequestController : ApiBaseController
    {
        private readonly IMediator mediator;

        public PatientScanRequestController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = SD.DoctorRole)]
        [HttpPost("Doctor/CreateRequest")]
        public async Task<IActionResult> CreateRequest ([FromQuery] string PatientId, 
            [FromForm] AiModelTypeEnum AIModelType,
            string? DoctorNote,
            DateTime? ExpirationDate, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new CreateScanRequestCommand(PatientId,AIModelType,DoctorNote,ExpirationDate),cancellationToken);

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }
        [Authorize(Roles = SD.PatientRole)]
        [HttpPost("Patient/UploadImages")]
        public async Task<IActionResult> UploadImages([FromQuery] string ScanRequestId, 
            [FromForm] List<IFormFile> Images,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new UploadRequestedScanCommand(ScanRequestId,Images),cancellationToken);

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = SD.DoctorRole)]
        [HttpGet("Doctor/GetScanDetails")]
        public async Task<IActionResult> GetDetails(string scanRequestId,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetScanRequestDetailsQuery(scanRequestId),cancellationToken);

            if (!result.ISucsses)
                return NotFound(result);

            return Ok(result);
        }


        [Authorize(Roles = SD.DoctorRole)]
        [HttpPut("Doctor/approve")]
        public async Task<IActionResult> Approve([FromQuery]string ScanRequestId,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new ApproveUploadedScanCommand(ScanRequestId),cancellationToken);

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = SD.DoctorRole)]
        [HttpPut("Doctor/reject")]
        public async Task<IActionResult> Reject([FromQuery]string ScanRequestId,
        [FromBody]string RejectReason,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new RejectUploadedScanCommand(ScanRequestId,RejectReason),cancellationToken);

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = SD.DoctorRole)]
        [HttpPut("Doctor/cancel")]
        public async Task<IActionResult> Cancel([FromQuery]string scanRequestId, string? CancelReason,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new CancelScanRequestCommand(scanRequestId,CancelReason),cancellationToken);

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }


        [Authorize(Roles = SD.DoctorRole)]
        [HttpPost("Doctor/AnalysisScanRequest")]
        public async Task<IActionResult> AnalysisScanRequest(
    [FromQuery]string scanRequestId,
    CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new AnalyzeRequestedScanCommand(
                    scanRequestId
                    ),cancellationToken);

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }


        [Authorize(Roles = SD.DoctorRole)]
        [HttpPost("Doctor/publish")]
        public async Task<IActionResult> Publish(
    [FromQuery]string scanRequestId,
    string? DoctorNote,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new PublishAiReportCommand(
                    scanRequestId,
                    DoctorNote),cancellationToken);

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = SD.DoctorRole)]
        [HttpGet("Doctor/GetAllDoctorRequests")]
        public async Task<IActionResult> GetDoctorRequests(
    [FromQuery] ScanRequestStatus? status,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetAllDoctorScanRequestsQuery(status),cancellationToken);

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

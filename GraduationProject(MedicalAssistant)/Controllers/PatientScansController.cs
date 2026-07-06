using Features.PatientMedicalScanFeature.Commands;
using Features.PatientMedicalScanFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;
using Utility;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    public class PatientScansController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public PatientScansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Patient uploads a medical scan image. The image is validated for quality (blur, brightness, contrast, resolution).
        /// If quality passes, the scan is saved and set to PendingDoctorReview status.
        /// </summary>
        [HttpPost("upload")]
        [Authorize(Roles = $"{SD.PatientRole}")]
        public async Task<IActionResult> UploadScan(
            [FromForm] IFormFile ScanFile,
            [FromForm] string DoctorId,
            [FromForm] AiModelTypeEnum ModelType)
        {
            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(patientId))
                return Unauthorized("Patient identity not found.");

            var result = await _mediator.Send(new UploadPatientScanCommand(patientId, DoctorId, ModelType, ScanFile));

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Doctor triggers AI analysis on a previously uploaded and quality-validated scan.
        /// </summary>
        [HttpPost("{scanId}/analyze")]
        [Authorize(Roles = $"{SD.DoctorRole}, {SD.AdminRole}, {SD.SuperAdminRole}")]
        public async Task<IActionResult> AnalyzeScan(string scanId, [FromBody] AnalyzeScanRequest? request)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(doctorId))
                return Unauthorized("Doctor identity not found.");

            var result = await _mediator.Send(new AnalyzePatientScanCommand(scanId, doctorId, request?.DoctorNote));

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Doctor approves the AI-generated report so that the patient can see the final result.
        /// </summary>
        [HttpPost("{scanId}/approve")]
        [Authorize(Roles = $"{SD.DoctorRole}, {SD.AdminRole}, {SD.SuperAdminRole}")]
        public async Task<IActionResult> ApproveScanReport(string scanId, [FromBody] ApproveScanRequest? request)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(doctorId))
                return Unauthorized("Doctor identity not found.");

            var result = await _mediator.Send(new ApprovePatientScanReportCommand(scanId, doctorId, request?.DoctorNote));

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Doctor retrieves all scans assigned to them that are pending review or analyzed (awaiting approval).
        /// </summary>
        [HttpGet("doctor/pending")]
        [Authorize(Roles = $"{SD.DoctorRole}, {SD.AdminRole}, {SD.SuperAdminRole}")]
        public async Task<IActionResult> GetPendingScans()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(doctorId))
                return Unauthorized("Doctor identity not found.");

            var result = await _mediator.Send(new GetPendingScansForDoctorQuery(doctorId));

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves full details for a specific scan, including quality scores, AI report, and doctor notes.
        /// Accessible by the assigned doctor or the patient who uploaded the scan.
        /// </summary>
        [HttpGet("{scanId}")]
        [Authorize(Roles = $"{SD.PatientRole}, {SD.DoctorRole}, {SD.AdminRole}, {SD.SuperAdminRole}")]
        public async Task<IActionResult> GetScanDetails(string scanId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User identity not found.");

            var result = await _mediator.Send(new GetPatientScanDetailsQuery(scanId, userId));

            if (!result.ISucsses)
                return BadRequest(result);

            return Ok(result);
        }
    }

    // Simple request DTOs for JSON body parameters
    public class AnalyzeScanRequest
    {
        public string? DoctorNote { get; set; }
    }

    public class ApproveScanRequest
    {
        public string? DoctorNote { get; set; }
    }
}

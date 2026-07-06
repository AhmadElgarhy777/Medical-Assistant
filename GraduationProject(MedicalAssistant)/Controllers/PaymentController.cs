using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Services.PaymentServices;
using Features.PharmacyFeature;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        // ✅ المريض يدفع Invoice
        [HttpPost("pay/{invoiceId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Pay(string invoiceId)
        {
            // 1. جيب الـ Invoice
            var invoice = await _orderService.GetInvoiceByIdAsync(invoiceId);
            if (invoice == null)
                return NotFound("الـ Invoice مش موجود!");

            if (invoice.PaymentStatus == "Paid")
                return BadRequest("الـ Invoice ده اتدفع قبل كده!");

            // 2. جيب الـ Auth Token
            var authToken = await _paymentService.GetAuthTokenAsync();

            // 3. عمل Order في Paymob
            var paymobOrderId = await _paymentService.CreateOrderAsync(authToken, invoice.TotalAmount);

            // 4. جيب الـ Payment Key
            var paymentKey = await _paymentService.GetPaymentKeyAsync(authToken, paymobOrderId, invoice.TotalAmount);

            // 5. رجع رابط الدفع
            var paymentUrl = _paymentService.GetPaymentUrl(paymentKey);

            return Ok(new { paymentUrl });
        }
    }
}
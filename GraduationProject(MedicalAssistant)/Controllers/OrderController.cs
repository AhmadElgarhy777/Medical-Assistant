using AutoMapper;
using Features.PharmacyFeature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using System.Security.Claims;

namespace GraduationProject_MedicalAssistant_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper mapper;

        public OrderController(IOrderService orderService,IMapper mapper)
        {
            _orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                return BadRequest("مفيش أدوية في الطلب!");

            var result = await _orderService.CreateOrderAsync(dto);
            return Ok(result);
        }

      

        [HttpPut("UpdateOrderStatus")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> UpdateOrderStatus(string orderId, [FromQuery] string status)
        {
            if (string.IsNullOrEmpty(status))
                return BadRequest("ادخل الحالة!");

            var validStatuses = new[] { "Pending", "Confirmed", "Delivered", "Cancelled" };
            if (!validStatuses.Contains(status))
                return BadRequest("الحالة مش صحيحة! لازم تكون: Pending, Confirmed, Delivered, Cancelled");

            var result = await _orderService.UpdateOrderStatusAsync(orderId, status);
            if (result == null)
                return NotFound("الـ Order مش موجود!");

            return Ok(result);
        }

       

     

        // ✅ تفاصيل طلب واحد
        [HttpGet("{orderId}")]
        [Authorize(Roles = "Pharmacy,Patient")]
        public async Task<IActionResult> GetOrderById(string orderId)
        {
            var result = await _orderService.GetOrderByIdAsync(orderId);
            if (result == null)
                return NotFound("الطلب مش موجود!");

            var resultDto=new OrderResultDto
            {
                OrderId = result.ID,
                TotalAmount = result.TotalAmount,
                Status = result.Status,
                InvoiceId = result.Invoice.ID,
                InvoiceTotalAmount = result.Invoice.TotalAmount,
                PaymentStatus = result.Invoice.PaymentStatus
            };
            return Ok(resultDto);
        }

        [HttpPut("invoice/{orderId}")]
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> UpdateInvoicePayment(
            string orderId,
            [FromQuery] string paymentStatus,
            [FromQuery] string paymentMethod)
        {
            if (string.IsNullOrEmpty(paymentStatus) || string.IsNullOrEmpty(paymentMethod))
                return BadRequest("ادخل حالة الدفع وطريقة الدفع!");

            var validStatuses = new[] { "Unpaid", "Paid", "Refunded" };
            if (!validStatuses.Contains(paymentStatus))
                return BadRequest("حالة الدفع مش صحيحة! لازم تكون: Unpaid, Paid, Refunded");

            var validMethods = new[] { "Cash", "Card", "Online" };
            if (!validMethods.Contains(paymentMethod))
                return BadRequest("طريقة الدفع مش صحيحة! لازم تكون: Cash, Card, Online");

            var result = await _orderService.UpdateInvoicePaymentAsync(orderId, paymentStatus, paymentMethod);
            if (result == null)
                return NotFound("الـ Invoice مش موجود!");

            return Ok(result);
        }

        [HttpPut("cancel/{orderId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            try
            {
                var result = await _orderService.CancelOrderAsync(orderId);
                if (result == null)
                    return NotFound("الـ Order مش موجود!");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
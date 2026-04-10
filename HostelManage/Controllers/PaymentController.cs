
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using HostelManage.Services;
//using System;
//using System.Security.Claims;

//namespace HostelManage.Controllers
//{
//    [ApiController]
//    [Route("api/payment")]
//    public class PaymentController : ControllerBase
//    {
//        private readonly IPaymentService _paymentService;

//        public PaymentController(IPaymentService paymentService)
//        {
//            _paymentService = paymentService;
//        }

//        /// <summary>
//        /// Initiates a payment and returns the Khalti payment URL.
//        /// </summary>
//        [HttpPost("initiate")]
//        public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequestModel request)
//        {
//            if (request == null || request.Amount <= 0 || string.IsNullOrEmpty(request.OrderId))
//            {
//                return BadRequest(new { message = "Invalid payment request" });
//            }

//            try
//            {
//                var paymentUrl = await _paymentService.InitiatePaymentAsync(
//                    request.Amount, request.OrderId, request.OrderName,
//                    request.CustomerName, request.CustomerEmail, request.CustomerPhone
//                );

//                if (string.IsNullOrEmpty(paymentUrl))
//                {
//                    return BadRequest(new { message = "Failed to initiate payment" });
//                }

//                return Ok(new { paymentUrl });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = "An error occurred while initiating payment", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// Verifies a payment when Khalti redirects to this API.
//        /// </summary>
//        [HttpPost("verify-payment")]  // ✅ Ensure it's POST since your frontend is sending POST
//        public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentRequest request)
//        {
//            if (request == null || string.IsNullOrEmpty(request.Pidx) || request.Amount <= 0)
//            {
//                return BadRequest(new { message = "Invalid payment request" });  // ✅ Improve error message
//            }

//            bool isVerified = await _paymentService.VerifyPaymentAsync(request.Pidx, request.Amount);

//            if (!isVerified)
//            {
//                return BadRequest(new { message = "Payment verification failed" });
//            }

//            return Ok(new { message = "Payment verified successfully" });
//        }

//        // ✅ Create a request model
//        public class VerifyPaymentRequest
//        {
//            public string Pidx { get; set; }
//            public decimal Amount { get; set; }
//        }

//    }


//    /// <summary>
//    /// Model for payment initiation request.
//    /// </summary>
//    public class PaymentRequestModel
//    {
//        public decimal Amount { get; set; }
//        public string OrderId { get; set; }
//        public string OrderName { get; set; }
//        public string CustomerName { get; set; }
//        public string CustomerEmail { get; set; }
//        public string CustomerPhone { get; set; }
//    }

//    /// <summary>
//    /// Model for payment verification request.
//    /// </summary>
//    public class PaymentVerificationRequest
//    {
//        public string pidx { get; set; }

//        public decimal amount { get; set; }

//    }
//}

//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using System;
//using HostelManage.Application.Services;

//namespace HostelManage.Controllers
//{
//    [ApiController]
//    [Route("api/payment")]
//    public class PaymentController : ControllerBase
//    {
//        private readonly IPaymentService _paymentService;

//        public PaymentController(IPaymentService paymentService)
//        {
//            _paymentService = paymentService;
//        }

//        [HttpPost("initiate")]
//        public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequestModel request)
//        {
//            if (request == null || request.Amount <= 0 || string.IsNullOrEmpty(request.OrderId))
//            {
//                return BadRequest(new { message = "Invalid payment request" });
//            }

//            try
//            {
//                var paymentUrl = await _paymentService.InitiatePaymentAsync(
//                    request.Amount,
//                    request.OrderId,
//                    request.OrderName
//                );

//                if (string.IsNullOrEmpty(paymentUrl))
//                {
//                    return BadRequest(new { message = "Failed to initiate payment" });
//                }

//                return Ok(new { paymentUrl });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new
//                {
//                    message = "Error initiating payment",
//                    error = ex.Message
//                });
//            }
//        }

//        //[HttpPost("verify-payment")]
//        //public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentRequest request)
//        //{
//        //    if (request == null || string.IsNullOrEmpty(request.Pidx))
//        //    {
//        //        return BadRequest(new { message = "Invalid verification request" });
//        //    }

//        //    bool isVerified = await _paymentService.VerifyPaymentAsync(request.Pidx);

//        //    if (!isVerified)
//        //    {
//        //        return BadRequest(new { message = "Payment verification failed" });
//        //    }

//        //    return Ok(new { message = "Payment verified successfully" });
//        //}

//        //[HttpPost("verify-payment")]
//        //public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentRequest request)
//        //{
//        //    if (request == null || string.IsNullOrEmpty(request.Pidx))
//        //    {
//        //        return BadRequest(new { message = "Invalid request" });
//        //    }

//        //    bool isVerified = await _paymentService.VerifyPaymentAsync(request.Pidx);

//        //    if (!isVerified)
//        //    {
//        //        return BadRequest(new { message = "Payment verification failed" });
//        //    }

//        //    return Ok(new { message = "Payment verified successfully" });
//        //}

//        [HttpPost("verify-payment")]
//        public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentRequest request)
//        {
//            if (request == null || string.IsNullOrEmpty(request.Pidx))
//            {
//                return BadRequest(new { message = "Invalid request" });
//            }

//            bool isVerified = await _paymentService.VerifyPaymentAsync(request.Pidx);

//            if (!isVerified)
//            {
//                return BadRequest(new { message = "Payment verification failed" });
//            }

//            // 🔥 NEW: update booking + room
//            await _paymentService.MarkBookingAsPaid(request.BookingId);

//            return Ok(new { message = "Payment verified & booking updated" });
//        }

//        public class VerifyPaymentRequest
//        {
//            public string Pidx { get; set; }

//            public int BookingId { get; set; }

//        }
//    }

//    public class PaymentRequestModel
//    {
//        public decimal Amount { get; set; }
//        public string OrderId { get; set; }
//        public string OrderName { get; set; }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HostelManage.Application.Interfaces;
using HostelManage.Application.DTOs.Payment;

namespace HostelManage.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IBookingService _bookingService;

        public PaymentController(IPaymentService paymentService, IBookingService bookingService)
        {
            _paymentService = paymentService;
            _bookingService = bookingService;
        }

        // 🔹 Initiate Payment
        [HttpPost("initiate")]
        public async Task<IActionResult> InitiatePayment([FromBody] InitiatePaymentDTO request)
        {
            if (request == null || request.BookingId <= 0)
                return BadRequest(new { message = "Invalid bookingId" });

            try
            {
                var paymentUrl = await _paymentService.InitiatePaymentAsync(request.BookingId);

                if (string.IsNullOrEmpty(paymentUrl))
                    return BadRequest(new { message = "Failed to initiate payment" });

                return Ok(new { paymentUrl });
            }
            catch
            {
                return StatusCode(500, new { message = "Error initiating payment" });
            }
        }

        // 🔹 Verify Payment
        [HttpPost("verify-payment")]
        public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Pidx) || request.BookingId <= 0)
                return BadRequest(new { message = "Invalid request" });

            try
            {
                var isVerified = await _paymentService.VerifyPaymentAsync(request.Pidx);

                if (!isVerified)
                    return BadRequest(new { message = "Payment verification failed" });

                await _bookingService.MarkBookingAsPaid(request.BookingId);

                return Ok(new { message = "Payment verified & booking updated" });
            }
            catch
            {
                return StatusCode(500, new { message = "Error verifying payment" });
            }
        }
    }
}
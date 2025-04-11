using EventManagementSystem_Merged_.Repos;
using EventManagementSystemMerged.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        public PaymentController(PaymentService paymentService)
        {

            _paymentService = paymentService;

        }

        [HttpPost("complete-payment/{paymentId}")]

        public ActionResult CompletePayment(int paymentId)

        {

            _paymentService.GenerateTicketIfPaymentCompleted(paymentId);

            return Ok();

        }

        [HttpGet]

        public ActionResult<List<Payment>> GetPayments()
        {
            var payments = _paymentService.GetAllPayments();
            return Ok(payments);
        }

        [HttpPost]
        public ActionResult<Payment> CreatePayment([FromForm] Payment newPayment)
        {
            if (newPayment == null)
            {
                return BadRequest("Payment is null.");
            }

            _paymentService.CreatePayment(newPayment);
            return CreatedAtAction(nameof(GetPayments), new { id = newPayment.PaymentID }, newPayment);
        }


    }

}

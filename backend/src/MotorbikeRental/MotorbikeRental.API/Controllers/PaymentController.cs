using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorbikeRental.Application.DTOs.Payments;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.Interface.IServices.IContractServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace MotorbikeRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [Authorize(Roles = "Receptionist")]
        [HttpPost("{id}/preview-payment")]
        public async Task<IActionResult> PreviewPayment(int id, CancellationToken cancellationToken = default)
        {
            var result = await paymentService.PreviewPayment(id, cancellationToken);

            var response = new ResponseDto<PaymentPreviewDto>
            {
                Success = true,
                Message = "Payment preview retrieved successfully",
                Data = result
            };

            return Ok(response);
        }

        [Authorize(Roles = "Receptionist")]
        [HttpPost("process-payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentProcessDto paymentProcessDto,
            CancellationToken cancellationToken = default)
        {
            var result = await paymentService.ProcessPayment(paymentProcessDto, cancellationToken);

            var response = new ResponseDto<PaymentDto>
            {
                Success = true,
                Message = "Payment processed successfully",
                Data = result
            };

            return Ok(response);
        }

        [Authorize(Roles = "Manager,Receptionist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByContractId(int id, CancellationToken cancellationToken = default)
        {
            var result = await paymentService.GetById(id, cancellationToken);

            var response = new ResponseDto<PaymentDto>()
            {
                Success = true,
                Message = "Payment retrieved successfully",
                Data = result
            };

            return Ok(response);
        }
    }
}
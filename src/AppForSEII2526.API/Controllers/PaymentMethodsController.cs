using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.DTOs.ApplicationUserDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Identity.Client.AppConfig;



namespace AppForSEII2526.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PaymentMethodsController> _logger;

        public PaymentMethodsController(ApplicationDbContext context, ILogger<PaymentMethodsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<PaymentMethodForPurchaseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> GetPaymentMethods(string username)
        {
            List<PaymentMethodForPurchaseDTO>? paymentMethods = await _context.PaymentMethods
                .Where(p => p.User.UserName == username)
                .Select(p => new PaymentMethodForPurchaseDTO(
                    p.Id,
                    p.GetType().Name,
                    p.ToString()
                    ))
                .ToListAsync();

            if (paymentMethods == null)
            {
                NotFound(paymentMethods);
            }


            return Ok(paymentMethods);
        }
    }
}
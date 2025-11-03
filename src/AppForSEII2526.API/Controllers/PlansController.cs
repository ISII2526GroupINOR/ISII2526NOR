using AppForSEII2526.API.DTOs.ApplicationUserDTOs;
using AppForSEII2526.API.DTOs.ClassDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        // Used to enable the controller to access the database
        private readonly ApplicationDbContext _context;
        // Used to log any information when the system is running
        private readonly ILogger<PlansController> _logger;

        public PlansController(ApplicationDbContext context, ILogger<PlansController> logger)
        {
            _context = context;
            _logger = logger;
        }


        // HELP
        // - Which properties of the different entities need the null-forgiving operator?
        // - How to properly check for null values in nested relationships when projecting to DTOs?
        // - Should all properties of the DTOs be nullable to avoid potential issues during projection?
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PlanDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPlanDetails(int planId)
        {
            PlanDetailDTO? plan = await _context.Plans
                .Where(p => p.Id == planId)
                .Include(p => p.PaymentMethod)
                    .ThenInclude(pm => pm.User)
                .Include(p => p.PlanItems)
                    .ThenInclude(pi => pi.Class)
                .Select(p => new PlanDetailDTO(
                    p.Name,
                    p.Description,
                    p.CreatedDate,
                    p.HealthIssues, // Null-forgiving operator is not needed as HealthIssues is nullable
                    p.TotalPrice,
                    p.Weeks,
                    new ApplicationUserForPlanDetailDTO(
                        p.PaymentMethod.User.Id,
                        p.PaymentMethod.User.Name,
                        p.PaymentMethod.User.Surname),
                    p.PlanItems.Select(pi => new ClassForPlanDTO(
                        pi.Class.Id,
                        pi.Class.Name,
                        pi.Class.Price,
                        pi.Class.TypeItems.Select(ti => ti.Name).ToList(),
                        pi.Class.Date
                    )).ToList()
                )).FirstOrDefaultAsync();

            if (plan == null)
            {
                _logger.LogError($"Error: Plan with id {planId} does not exist");
                return NotFound();
            }

            return Ok(plan);
        }
    }
}

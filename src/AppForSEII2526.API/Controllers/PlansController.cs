using AppForSEII2526.API.DTOs.ApplicationUserDTOs;
using AppForSEII2526.API.DTOs.ClassDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.DTOs.RestockDTOs;
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

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PlanDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]

        public async Task<ActionResult> PostCreatePlan(PlanForCreateDTO planForCreate)
        {
            // Name is required, but managed by model validation

            // Weeks validation
            if (planForCreate.Weeks <= 0)
                ModelState.AddModelError("Weeks", "Error! Weeks must be greater than zero.");

            // PaymentMethodId validation and check for coherence with UserId
            if(await _context.PaymentMethods.FindAsync(planForCreate.PaymentMethodId) == null)
            {
                ModelState.AddModelError("PaymentMethodId", $"Error! Payment method with id {planForCreate.PaymentMethodId} does not exist.");
            }
            else
            {
                var paymentMethod = await _context.PaymentMethods
                    .Include(pm => pm.User)
                    .FirstOrDefaultAsync(pm => pm.Id == planForCreate.PaymentMethodId);
                if (paymentMethod!.User == null || paymentMethod!.User.Id != planForCreate.UserId)
                {
                    ModelState.AddModelError("PaymentMethodId", $"Error! Payment method with id {planForCreate.PaymentMethodId} does not belong to user with id {planForCreate.UserId}.");
                }
            }

            // Check for repeated plan name
            var repeated_plans = await _context.Plans.Where(p => p.Name == planForCreate.Name).ToListAsync();
            if(repeated_plans.Count > 0)
            {
                ModelState.AddModelError("PlanName", $"Error! Plan with name {planForCreate.Name} already exists");
            }

            // Classes validation
            foreach (var classForCreate in planForCreate.classes)
            {
                
                // Check if class exists in the database
                if (_context.Classes.Find(classForCreate.Id) == null)
                {
                    ModelState.AddModelError("Classes", $"Error! Class with id {classForCreate.Id} does not exist.");
                }

                // Check for goal property and replace with default if null. NOT ACCORDING TO REQUIREMENTS
                //if (classForCreate.goal == null)
                //{
                //    classForCreate.goal = "Have fun!";
                //}

                // EXAM: Check goal as defined in issue #128
                if(classForCreate.goal != null && !(classForCreate.goal.StartsWith("I would like to") ))
                {
                    ModelState.AddModelError("Goal", "Error!, You must start the description of your goals with I would like to");
                }

                // Check for duplicate classes in the plan
                if (planForCreate.classes.Count(c => c.Id == classForCreate.Id) > 1)
                {
                    ModelState.AddModelError("Classes", $"Error! Duplicate class with id {classForCreate.Id} found in the plan.");
                }

                // Check if class capacity is exceeded
                var classEntity = await _context.Classes
                    .Include(c => c.PlanItems)
                    .FirstOrDefaultAsync(c => c.Id == classForCreate.Id);
                if (classEntity != null && classEntity.PlanItems.Count >= classEntity.Capacity)
                {
                    ModelState.AddModelError("Classes", $"Error! Class with id {classForCreate.Id} has reached its capacity limit.");
                }

                // Check if class date is in the past
                if (classEntity != null && classEntity.Date < DateTime.Now)
                {
                    ModelState.AddModelError("Classes", $"Error! Class with id {classForCreate.Id} is scheduled in the past.");
                }
            }


            // Throw errors if any
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Calculate total price
            decimal totalPrice = 0;
            foreach (var classForCreate in planForCreate.classes)
            {
                var classEntity = await _context.Classes.FindAsync(classForCreate.Id);
                if (classEntity != null)
                {
                    totalPrice += classEntity.Price;
                }
            }
            if (totalPrice <= 0)
            {
                ModelState.AddModelError("TotalPrice", "Error! Total price must be greater than zero.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Create Plan entity

            Plan plan = new Plan
            {
                Name = planForCreate.Name,
                Description = planForCreate.Description,
                CreatedDate = DateTime.Now,
                HealthIssues = planForCreate.HealthIssues,
                TotalPrice = totalPrice,
                Weeks = planForCreate.Weeks,
                PaymentMethod = await _context.PaymentMethods.FindAsync(planForCreate.PaymentMethodId),
                PlanItems = new List<PlanItem>()
            };

            // create PlanItems and add to Plan
            foreach (var classForCreate in planForCreate.classes)
            {
                
                var classEntity = await _context.Classes.FindAsync(classForCreate.Id);
                
                PlanItem planItem = new PlanItem // Create new PlanItem without constructor because goal is required and the constructor gives problems
                {
                    Plan = plan,
                    Class = classEntity,
                    Goal = classForCreate.goal!,
                    Price = classEntity.Price
                };
                plan.PlanItems.Add(planItem);
            }

            // No additional errors will be produced before inserting to the database

            // INSERT into database
            _context.Plans.Add(plan);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Plan", "There was a problem while submitting the class, please, try again later");
                return Conflict("Error" + ex.Message);
            }


            // Prepare response DTO
            var returnPlanDetail = await _context.Plans
                .Where(p => p.Id == plan.Id)
                .Include(p => p.PaymentMethod)
                    .ThenInclude(pm => pm.User)
                .Include(p => p.PlanItems)
                    .ThenInclude(pi => pi.Class)
                .Select(p => new PlanDetailDTO
                    (
                        p.Name,
                        p.Description,
                        p.CreatedDate,
                        p.HealthIssues,
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
                    )
                ).FirstOrDefaultAsync();

            // Return
            return CreatedAtAction(nameof(GetPlanDetails), new { planId = plan.Id }, returnPlanDetail);
        }
    }
}

using AppForSEII2526.API.DTOs.ClassDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Collections;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ClassesController> _logger;

        public ClassesController(ApplicationDbContext context, ILogger<ClassesController> logger)
        {
            // Uses Dependency injection
            _context = context; // Helps us to access the database
            _logger = logger; // Helps us to log information and errors
        }


        /// <summary>
        /// Auxiliary method to compute the number of days until the next occurrence of a specific day of the week.
        /// </summary>
        /// <param name="desiredDay">The desired day of the week.</param>
        /// <returns>Number of days until the next occurrence of the desired day of the week.</returns>
        private static int daysUntilNextDayOfWeek(DayOfWeek desiredDay)
        {
            var today = DateTime.Now.Date;
            int daysUntilNextDesiredDay = ((int)desiredDay - (int)today.DayOfWeek + 7) % 7;
            return daysUntilNextDesiredDay;
        }


        [HttpGet]
        [Route("[action]")] // api/Classes/GetClassesForPlan
        [ProducesResponseType(typeof(IList<ClassForPlanDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetAvailableClassesForPlan(string? className, DateOnly? date)
        {
            // Intermediate operations

            // Prepare the date range for the next week (Monday to Sunday)
            var today = DateTime.Now.Date;

            int daysUntilNextMonday = daysUntilNextDayOfWeek(DayOfWeek.Monday);
            if (daysUntilNextMonday == 0) // si hoy es lunes, queremos la siguiente semana
            {
                daysUntilNextMonday = 7;
            }

            DateOnly nextWeekMonday = DateOnly.FromDateTime(today.AddDays(daysUntilNextMonday));
            DateOnly nextWeekSunday = nextWeekMonday.AddDays(6);

            // Set to hold valid dates
            HashSet<DateOnly> validDates = new HashSet<DateOnly>(); // To store valid dates within the next week

            if (date != null)
            {
                // Check if the provided date is within the next week (Monday to Sunday)
                if (date.Value < nextWeekMonday || date.Value > nextWeekSunday)
                {
                    return BadRequest("The date must be within the next week (Monday to Sunday).");
                }

                // Add the specified date to the valid dates set
                validDates.Add(date.Value);
            }
            else
            {
                // Insert all dates from next Monday to next Sunday
                for (DateOnly dayElement = nextWeekMonday; dayElement <= nextWeekSunday; dayElement = dayElement.AddDays(1))
                {
                    validDates.Add(dayElement);
                }
            }


            IList<ClassForPlanDTO> classes = await _context.Classes
                .Include(c => c.PlanItems) // Eager loading of PlanItems
                .Where(c => (className == null || c.Name.ToLower().Contains(className.ToLower()) )
                    && c.PlanItems.Count < c.Capacity // Only classes with available capacity
                    && validDates.Contains(DateOnly.FromDateTime(c.Date.Date)) // Only classes within the valid dates set, calculated earlier
                )
                .Select(c => new ClassForPlanDTO(
                    c.Id,
                    c.Name,
                    c.Price,
                    c.TypeItems.Select(ti => ti.Name).ToList(), // Include List of TypeItem.Name instead of full TypeItem database objects
                    c.Date
                    )
                )
                .ToListAsync();

            return Ok(classes);
        }


        // We are going to transform this method into a service of the web API.
        // For that, it can return a Task<ActionResult> or IActionResult
        // It is a GET
        // It is called an action method
        //[HttpGet]
        //[Route("[action]")] // api/classes/ComputeDivision
        //[ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        //public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2) 
        //{
        //    if (op2 == 0) {
        //        string error = "Op2 cannot be 0 to compute a division";
        //        _logger.LogError(DateTime.Now + " Error: " + error);
        //        return BadRequest(error);
        //    }
        //    decimal result = op1 / op2; 
        //    return Ok(result);
        //}
    }
}

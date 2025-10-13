using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

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
            _context = context;
            _logger = logger;

            // Uses Dependency injection
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

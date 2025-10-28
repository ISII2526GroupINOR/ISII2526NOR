using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppForSEII2526.API.DTOs.RestockDTOs;
using AppForSEII2526.API.DTOs.ItemDTOs;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestocksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RestocksController> _logger;

        public RestocksController(ApplicationDbContext context, ILogger<RestocksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(RestockDetailDTO), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetRestock(int id)
        {
            if(_context.Restocks == null)
            {
                _logger.LogError("Error: Restock table does not exist");
                return NotFound();
            }
            RestockDetailDTO? restock = await _context.Restocks.
                Where(r => r.Id == id)
                    .Include(r => r.RestockItems)
                        .ThenInclude(ri => ri.Item)
                            .ThenInclude(item => item.Brand)
                .Select(r => new RestockDetailDTO(r.Id, r.Title, r.DeliveryAddress, r.Description,
                r.ExpectedDate, r.TotalPrice, r.RestockItems
                    .Select(ri => new ItemForRestockingDTO(ri.Item.Id, ri.Item.Name, ri.Item.Brand.Name, 
                    ri.RestockPrice, ri.Item.QuantityForRestock)).ToList<ItemForRestockingDTO>())
                ).FirstOrDefaultAsync();
            if (restock == null)
            {
                _logger.LogError($"Error: Restock with id {id} does not exist");
                return NotFound();
            }
            return Ok(restock);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(RestockDetailDTO), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateRestock(RestockForCreateDTO restockForCreateDTO)
        {
            if(restockForCreateDTO.Title == null)
                ModelState.AddModelError("RestockTitleFrom", "Error, a title must be specified.");

            if (restockForCreateDTO.DeliveryAddress == null)
                ModelState.AddModelError("RestockDeliveryAddressFrom", "Error, an address to deliver must be specified.");


            if (restockForCreateDTO.ExpectedDate != null && restockForCreateDTO.ExpectedDate <= DateTime.Now)
                ModelState.AddModelError("RestockDateFrom", "Error, the expected date must start later than today.");

        }
    }
}

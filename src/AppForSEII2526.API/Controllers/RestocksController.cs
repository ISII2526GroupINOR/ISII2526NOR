using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.DTOs.RestockDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            if (restockForCreateDTO.ExpectedDate != null && restockForCreateDTO.ExpectedDate <= DateTime.Now)
                ModelState.AddModelError("RestockDateFrom", "Error! The expected date must start later than today.");

            if (restockForCreateDTO.RestockItems.Count == 0)
                ModelState.AddModelError("RestockItem", "Error! At least one item must be selected for restock.");

            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == restockForCreateDTO.RestockResponsible.UserName);
            if (user == null)
                ModelState.AddModelError("RestockApplicationUser", "Error! User name is not registered");

            if(ModelState.Count > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            //contains the names of items to be restocked in the order
            var itemNames = restockForCreateDTO.RestockItems.Select(ri => ri.Name).ToList<string>();

            var items = _context.Items.Include(i => i.RestockItems)
                .Where(i => itemNames.Contains(i.Name))
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    i.Brand,
                    i.QuantityForRestock,
                    i.RestockPrice,
                    i.QuantityAvailableForPurchase
                }).ToList();

            Restock restock = new Restock(restockForCreateDTO.Title, restockForCreateDTO.DeliveryAddress, 
                restockForCreateDTO.Description, restockForCreateDTO.ExpectedDate, restockForCreateDTO.RestockDate,
                restockForCreateDTO.TotalPrice, new List<RestockItem>(), restockForCreateDTO.RestockResponsible);

            restock.TotalPrice = 0;

            foreach (var ritem in restockForCreateDTO.RestockItems)
            {
                var item = items.FirstOrDefault(i => i.Id == ritem.Id);
                if (ritem.Quantity + item.QuantityAvailableForPurchase > item.QuantityForRestock)
                    ModelState.AddModelError("RestockItem", $"Error! The total quantity for purchase plus the" +
                        $" quantity to restock of item {item.Name} must be bigger than the quantity for restock.");
                if (ritem.RestockPrice != null && restock.TotalPrice != null)
                {
                    ritem.RestockPrice = item.RestockPrice * ritem.Quantity;
                    restock.TotalPrice += ritem.RestockPrice;
                }
                else
                {
                    restock.TotalPrice = null;
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Restock", "There was a problem while saving your restock, please, try again later");
                return Conflict("Error" + ex.Message);
            }

            var restockDetail = new RestockDetailDTO(restock.Id, restock.Title, restock.DeliveryAddress, 
                restock.Description, restock.ExpectedDate, restock.TotalPrice, 
                restockForCreateDTO.RestockItems);

            return CreatedAtAction("GetRestock", new {id = restock.Id}, restockDetail);
        }
    }
}

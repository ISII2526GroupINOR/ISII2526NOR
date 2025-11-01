using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.DTOs.RestockDTOs;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            if (restockForCreateDTO.ExpectedDate <= DateTime.Now)
                ModelState.AddModelError("RestockDateFrom", "Error! The expected date must start later than today.");

            if (restockForCreateDTO.RestockItems.Count == 0)
                ModelState.AddModelError("RestockItem", "Error! At least one item must be selected for restock.");

            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == restockForCreateDTO.RestockResponsible);
            if (user == null)
                ModelState.AddModelError("RestockApplicationUser", "Error! User name is not registered");

            if(ModelState.Count > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            //contains the names of items to be restocked in the order
            var itemNames = restockForCreateDTO.RestockItems.Select(ri => ri.Id).ToList<int>();

            var items = _context.Items.Include(i => i.RestockItems)
                .Where(i => itemNames.Contains(i.Id))
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
                restockForCreateDTO.TotalPrice, new List<RestockItem>(), user!);

            restock.TotalPrice = 0;
            //To store new DTOs to be stored in the RestockDetailDTO that will be showned once the restock is created
            IList<ItemForRestockingDTO> ItemRestockDTO = new List<ItemForRestockingDTO>();

            foreach (var ritem in restockForCreateDTO.RestockItems)
            {
                if (ritem == null)
                {
                    ModelState.AddModelError("RestockItem", "The item to restock cannot be null.");
                    continue;
                }
                var item = items.FirstOrDefault(i => i.Id == ritem.Id);
                if (item == null)
                {
                    ModelState.AddModelError("Item", "The specified item cannot be found.");
                    continue;
                }
                var quantity = ritem.RestockQuantity + item.QuantityAvailableForPurchase;
                if (quantity < item.QuantityForRestock)
                    ModelState.AddModelError("RestockItem", $"Error! The total quantity for purchase {item.QuantityAvailableForPurchase} plus the" +
                        $" quantity to restock {ritem.RestockQuantity} of item {item.Name} must be bigger than the quantity for restock {item.QuantityForRestock}.");
                else
                {
                    if (item.RestockPrice != null && restock.TotalPrice != null) //If one item had price null, ignore the following
                    {
                        restock.TotalPrice += item.RestockPrice * ritem.RestockQuantity;
                    }
                    else
                    {
                        restock.TotalPrice = null;
                    }
                    var itemToRestock = await _context.Items.FindAsync(ritem.Id);
                    restock.RestockItems.Add(new RestockItem(ritem.RestockQuantity, item.RestockPrice * ritem.RestockQuantity, restock, itemToRestock));
                    ItemRestockDTO.Add(new ItemForRestockingDTO(ritem.Id, item.Name, item.Brand.Name, item.RestockPrice * ritem.RestockQuantity, item.QuantityForRestock, item.QuantityAvailableForPurchase));
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(restock);

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
                ItemRestockDTO);

            return CreatedAtAction("GetRestock", new {id = restock.Id}, restockDetail);
        }
    }
}

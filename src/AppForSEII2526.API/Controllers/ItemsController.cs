using AppForSEII2526.API.DTOs.ItemDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ItemsController> _logger;

        public ItemsController(ApplicationDbContext context, ILogger<ItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        //{
        //    {
        //        string error = "Op2 cannot be 0 to compute a division";
        //        return BadRequest(error);
        //    }
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ItemForPurchaseDTO>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(ModelError), (int)HttpStatusCode.BadRequest)]

        public async Task<ActionResult> GetItemsForPurchase(string? itemName, string? description, string? typeItem, string? brand)
        {
            IList<ItemForPurchaseDTO> itemsDTOs = await _context
                .Items
                .Include(i => i.TypeItem)
                .Include(i => i.PurchaseItems)
                    .ThenInclude(pi => pi.Purchase)
                .Where(i =>
                    (i.Name.Contains(itemName) || itemName == null) &&
                    (i.Description.Contains(description) || description == null) &&
                    (i.TypeItem.Name.Equals(typeItem) || typeItem == null) &&
                    (i.Brand.Name.Equals(brand) || brand == null) &&
                    (i.QuantityAvailableForPurchase > 0)
                )
                .OrderBy(i => i.Name)
                    .ThenBy(i => i.PurchasePrice)
                .Select(i => new ItemForPurchaseDTO(
                    i.Id, i.Name, i.TypeItem.Name, i.Brand.Name, i.Description,
                    i.QuantityAvailableForPurchase, i.PurchasePrice

                    ))
                .ToListAsync();

            return Ok(itemsDTOs);
        }



        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ItemForRestockingDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetItemsForRestocking(string? itemName, int? min, int? max)
        {
            IList<ItemForRestockingDTO> itemsDTOS = await _context.Items
                .Include(i => i.Brand)
                .Where(i => (i.Name.Contains(itemName) || itemName == null)
                    && (i.QuantityAvailableForPurchase < i.QuantityForRestock)
                    && (i.QuantityAvailableForPurchase >= min || min == null)
                    && (i.QuantityAvailableForPurchase <= max || max == null)
                )
                .OrderBy(i => i.Name)
                .Select(i => new ItemForRestockingDTO(i.Id, i.Name, i.Brand.Name, i.RestockPrice,
                    i.QuantityForRestock, i.QuantityAvailableForPurchase))
                .ToListAsync();
            return Ok(itemsDTOS);
        }
    }
}

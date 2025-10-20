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
        //[ProducesResponseType(typeof(string),(int)HttpStatusCode.BadRequest)]
        //public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        //{
        //    if(op2==0)
        //    {
        //        string error = "Op2 cannot be 0 to compute a division";
        //        _logger.LogError(DateTime.Now + "Error:" + error);
        //        return BadRequest(error);
        //    }
        //    decimal result = op1/op2;
        //    return Ok(result);
        //}
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ItemForRestockingDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetItemsForRestocking(string? itemName, int? min, int? max) {
            IList<ItemForRestockingDTO> itemsDTOS = await _context.Items
                .Include(i=>i.Brand) //not neccesary becuase it´s done automaticaly
                .Where(i=>(i.Name.Contains(itemName) || itemName == null)
                    && (i.QuantityAvailableForPurchase < i.QuantityForRestock)
                    && (i.QuantityAvailableForPurchase >= min || min == null)
                    && (i.QuantityAvailableForPurchase <= max || max == null))
                .OrderBy(i=>i.Name)
                .Select(i=>new ItemForRestockingDTO(i.Id, i.Name, i.Brand.Name, i.RestockPrice, i.QuantityForRestock, i.QuantityAvailableForPurchase))
                .ToListAsync();
            return Ok(itemsDTOS);
        }
    }
}

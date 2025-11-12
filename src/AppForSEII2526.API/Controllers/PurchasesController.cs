using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PurchasesController> _logger;

        public PurchasesController(ApplicationDbContext context, ILogger<PurchasesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        [HttpPost]
        [Route("[action]")]

        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreatePurchase(PurchaseForCreateDTO purchaseForCreate)
        {
            //any validation defined in PurchaseForCreate is checked before running the method so they don't have to be checked again
            
            /*
            //we must relate the Rental to the User
            var user = await _context.Users.FirstOrDefaultAsync(au => au.UserName == PurchaseDetailDTO.UserNameCustomer);
            if (user == null)
                ModelState.AddModelError("RentalApplicationUser", "Error! UserName is not registered");
            */

            //we must check that all the items to be purchased exist in the database 

            var itemIds = purchaseForCreate.Items.Select(i => i.Id).ToList<int>();

            var items = _context.Items.Include(pi => pi.PurchaseItems)
                .ThenInclude(i => i.Purchase)

                //we must check that all the movies to be rented exist in the database 
                .Where(i => itemIds.Contains(i.Id))

                //we use an anonymous type https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/anonymous-types
                .Select(i => new {
                    i.Id,
                    i.Name,
                    i.QuantityAvailableForPurchase,
                    i.PurchasePrice,

                })
                .ToList();



            //we must provide rental with the info to be saved in the database
            Purchase purchase = new Purchase(
                purchaseForCreate.Id,
                purchaseForCreate.City, purchaseForCreate.Country,
                purchaseForCreate.Street, DateTime.Now, purchaseForCreate.Description,
                purchaseForCreate.Total_price, purchaseForCreate.Items,
                purchaseForCreate.Payment_method);





            foreach (var pitem in purchaseForCreate.Items)
            {
                var item = items.FirstOrDefault(i => i.Id == i.Id);

                //we must check that there is enough quantity to be rented in the database
                if ((item == null) || (pitem.Quantity >= item.QuantityAvailableForPurchase))
                {
                    ModelState.AddModelError("RentalItems", $"Error! Item titled '{item.Name}' is not available in the quantity selected");
                }
                else
                {
                    // rental does not exist in the database yet and does not have a valid Id, so we must relate rentalitem to the object rental
                    purchase.PurchaseItems.Add(new PurchaseItem(
                        pitem, pitem.Id, purchase, purchase.Id, pitem.Quantity, pitem.PurchasePrice
                        )

                    item.PriceForRenting = movie.PriceForRenting;
                }
            }



            //if there is any problem because of the available quantity of movies or because any movie does not exist
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(purchase);

            try
            {
                //we store in the database both rental and its rentalitems
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(DateTime.Now + ":" + ex.Message);
                //ModelState.AddModelError("Rental", $"Error! There was an error while saving your rental, please, try again later");
                return Conflict("Error" + ex.Message);

            }

            var purchaseDetail = new PurchaseDetailDTO(purchase.Id, purchase.Description, 
                purchase.Street, purchase.City, purchase.Country, purchase.TotalPrice,
                purchase.paymentMethod, purchase.PurchaseItems
                )


            return CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchaseDetail);

        }
    }
}

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


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> GetPurchase(int id)
        {
            PurchaseDetailDTO? purchase = await _context.Purchases
                .Where(p => p.Id == id)
                .Include(p => p.PurchaseItems)
                    .ThenInclude(pi => pi.Item)
                        .ThenInclude(item => item.Brand)
                .Include(p => p.paymentMethod) // <-- add this
                .Select(p => new PurchaseDetailDTO(
                    p.Id,
                    p.Description,
                    p.Street,
                    p.City,
                    p.Country,
                    p.TotalPrice,
                    p.paymentMethod != null ? p.paymentMethod.Id : 0, // safe access
                    p.PurchaseItems.Select(pi => new ItemForPurchaseCreateDTO(
                        pi.Item.Id,
                        pi.Item.Name,
                        pi.Item.Brand.Name,
                        pi.Item.Description,
                        pi.AmountBought,
                        pi.Item.PurchasePrice
                    )).ToList()
                ))
                .FirstOrDefaultAsync();


            if (purchase == null)
            {
                _logger.LogError($"Error: Purchase with id {id} does not exist");
                return NotFound();
            }


            return Ok(purchase);
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

                .ToList();



            PaymentMethod paymentMethod = await _context.PaymentMethods.FindAsync(purchaseForCreate.Payment_methodId);


            if (paymentMethod == null)
            {
                ModelState.AddModelError("PaymentMethodId", $"Payment method with id {purchaseForCreate.Payment_methodId} not found");
            }

            //we must provide rental with the info to be saved in the database
            Purchase purchase = new Purchase(
                purchaseForCreate.Id,
                purchaseForCreate.City, purchaseForCreate.Country,
                purchaseForCreate.Street, DateTime.Now, purchaseForCreate.Description,
                purchaseForCreate.Total_price, new List<PurchaseItem>(),
                paymentMethod
                );


            decimal totalPrice = 0;

            IList<ItemForPurchaseCreateDTO> itemsForPurchase = new List<ItemForPurchaseCreateDTO>();


            foreach (var pitem in purchaseForCreate.Items)
            {
                Item item = items.FirstOrDefault(i => i.Id == i.Id);

                //we must check that there is enough quantity to be rented in the database
                if ((item == null) || (pitem.Quantity >= item.QuantityAvailableForPurchase))
                {
                    ModelState.AddModelError("PurchaseItems", $"Error! Item titled '{item.Name}' is not available in the quantity selected");
                }
                else
                {
                    
                    // rental does not exist in the database yet and does not have a valid Id, so we must relate rentalitem to the object rental
                    purchase.PurchaseItems.Add(new PurchaseItem(
                         item, pitem.Id, purchase, purchase.Id, pitem.Quantity, pitem.PurchasePrice
                        ));

                    itemsForPurchase.Add(new ItemForPurchaseCreateDTO(pitem.Id, pitem.Name,
                        pitem.Brand, pitem.Description, pitem.Quantity, pitem.PurchasePrice));

                    totalPrice += pitem.PurchasePrice * pitem.Quantity;


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
                purchase.Street, purchase.City, purchase.Country, totalPrice,
                purchase.paymentMethod.Id, itemsForPurchase
                );


            return CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchaseDetail);

        }
    }
}

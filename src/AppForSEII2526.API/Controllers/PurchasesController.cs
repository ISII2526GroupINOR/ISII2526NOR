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
        public async Task<ActionResult> CreatePurchase(PurchaseDetailDTO purchaseDetail)
        {
            //any validation defined in PurchaseForCreate is checked before running the method so they don't have to be checked again
            

            //we must relate the Rental to the User
            var user = await _context.Users.FirstOrDefaultAsync(au => au.UserName == PurchaseDetailDTO.UserNameCustomer);
            if (user == null)
                ModelState.AddModelError("RentalApplicationUser", "Error! UserName is not registered");


            //we must check that all the movies to be rented exist in the database 

            var movieTitles = rentalForCreate.RentalItems.Select(ri => ri.Title).ToList<string>();

            var movies = _context.Movies.Include(m => m.RentalItems)
                .ThenInclude(ri => ri.Rental)

                //we must check that all the movies to be rented exist in the database 
                .Where(m => movieTitles.Contains(m.Title))

                //we use an anonymous type https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/anonymous-types
                .Select(m => new {
                    m.Id,
                    m.Title,
                    m.QuantityForRental,
                    m.PriceForRenting,

                    //we count the number of rentalItems that are within the rental period 
                    NumberOfRentedItems = m.RentalItems.Count(ri => ri.Rental.RentalDateFrom <= rentalForCreate.RentalDateTo
                            && ri.Rental.RentalDateTo >= rentalForCreate.RentalDateFrom)
                })
                .ToList();



            //we must provide rental with the info to be saved in the database
            Rental rental = new Rental(rentalForCreate.DeliveryAddress, rentalForCreate.NameCustomer,
                rentalForCreate.SurnameCustomer, user, rentalForCreate.RentalDateFrom, rentalForCreate.RentalDateTo,
                rentalForCreate.PaymentMethod, new List<RentalItem>());




            foreach (var item in rentalForCreate.RentalItems)
            {
                var movie = movies.FirstOrDefault(m => m.Title == item.Title);
                //we must check that there is enough quantity to be rented in the database
                if ((movie == null) || (movie.NumberOfRentedItems >= movie.QuantityForRental))
                {
                    ModelState.AddModelError("RentalItems", $"Error! Movie titled '{item.Title}' is not available for being rented from {rentalForCreate.RentalDateFrom.ToShortDateString()} to {rentalForCreate.RentalDateTo.ToShortDateString()}");
                }
                else
                {
                    // rental does not exist in the database yet and does not have a valid Id, so we must relate rentalitem to the object rental
                    rental.RentalItems.Add(new RentalItem(movie.Id, rental, movie.PriceForRenting, item.Description));
                    item.PriceForRenting = movie.PriceForRenting;
                }
            }

            decimal numDays = (decimal)(rental.RentalDateTo - rental.RentalDateFrom).TotalDays;
            rental.CostofRental = rental.RentalItems.Sum(ri => ri.Price * numDays);

            //if there is any problem because of the available quantity of movies or because any movie does not exist
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(rental);

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

            var rentalDetail = new RentalForDetailDTO(rental.Id, rental.CostofRental,
                rental.RentalDate, rental.DeliveryAddress, rental.NameCustomer, rental.SurnameCustomer,
                rental.RentalDateFrom, rental.RentalDateTo, rental.PaymentMethod,

                rental.Customer.UserName!, rentalForCreate.RentalItems);

            return CreatedAtAction("GetRental", new { id = rental.Id }, rentalDetail);

        }
    }
}

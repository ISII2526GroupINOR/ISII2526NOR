using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.DTOs.RestockDTOs;

namespace AppForSEII2526.UT.RestocksController_test
{
    public class GestRestock_test : AppForSEII25264SqliteUT
    {
        public GestRestock_test()
        {
            var brand = new Brand("Precor");
            var typeItem = new TypeItem("Dumbbell");
            var items = new List<Item>
            {
                new Item("Dumbbell", "Regular dumbbell", 0, 8, 6, 30, brand, typeItem),
                new Item("Kettlebell", "Cicular dumbbell", 0, 5, 3, 40, brand, typeItem)
            };

            _context.AddRange(items);
            _context.SaveChanges();

            var user = new ApplicationUser("Jaime", "Domingo");

            var restock = new Restock("A restock", "An address", "A description", new DateTime(), new DateTime(),
                0, new List<RestockItem>(), user);
            restock.RestockItems.Add(new RestockItem(2, new Restock(), items[0]));
            restock.RestockItems.Add(new RestockItem(3, new Restock(), items[1]));

            _context.Users.Add(user);
            _context.Add(restock);
            _context.SaveChanges();
        }

        [Fact]
        [Trait ("Level Testing", "Unit Testing")]
        public async Task GetRestock_NotFound_test()
        {
            var mock = new Mock<ILogger<RestocksController>>();
            ILogger<RestocksController> logger = mock.Object;

            var controller = new RestocksController(_context, logger);

            var result = await controller.GetRestock(0);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait ("Level Testing", "Unit Testing")]
        public async Task GetRestock_Found_test()
        {
            var mock = new Mock<ILogger<RestocksController>>();
            ILogger<RestocksController> logger = mock.Object;

            var controller = new RestocksController(_context, logger);

            var expected = new RestockDetailDTO(1, "A restock", "An address", "A description", new DateTime(),
                0, new List<ItemForRestockingDTO>() { 
                    new ItemForRestockingDTO(1, "Dumbbell", "Precor", "Regular dumbbell"),
                    new ItemForRestockingDTO(2, "Kettlebell", "Precor", "Cicular dumbbell")});

            var result = await controller.GetRestock(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var restockDTOActual = Assert.IsType<RestockDetailDTO>(okResult.Value);

            Assert.Equal(expected, restockDTOActual);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ItemDTOs;

namespace AppForSEII2526.UT.ItemsController_test
{
    internal class GetItemsForPurchase_test : AppForSEII25264SqliteUT
    {
        public GetItemsForPurchase_test() {
            //fix these values
            var brands = new List<Brand>()
            {
                new Brand("El Corte Ingles"),
                new Brand("Decathlon")
            };

            var typeItems = new List<TypeItem>()
            {
                new TypeItem("Dumbell"),
                new TypeItem("Towel")
            };

            var items = new List<Item>()
            {
                new Item("5kg Dumbell", "", 10, 20, 20, 12, brands[0], typeItems[0]),
                new Item("Red Towel", "", 10, 20, 20, 12, brands[1], typeItems[1])
            };

            _context.AddRange(brands);
            _context.AddRange(items);
            _context.AddRange(typeItems);
            _context.SaveChanges();
        }

        [Fact]

        public async Task GetItemsForPurchaseNULL4NameBrand_test()
        {
            List<ItemForPurchaseDTO> expectedItems = new List<ItemForPurchaseDTO>()
            {
                new ItemForPurchaseDTO(1, "5kg Dumbbell", "Dumbell", "El Corte Ingles", "A dumbbell", 10, 2),
                new ItemForPurchaseDTO(2, "10kg Dumbbell", "Dumbell", "El Corte Ingles", "A dumbbell", 10, 3),
                //fix these items
            };

            var mock = new Mock<ILogger<ItemsController>>();
            ILogger<ItemsController> logger = mock.Object;
            ItemsController controller = new ItemsController(_context, logger);

            var result = await controller.GetItemsForPurchase(null, null, null, null);


            var okresult = Assert.IsType<OkObjectResult>(result);
            var itemsactualresult = Assert.IsType<List<ItemForPurchaseDTO>>(okresult.Value);

            Assert.Equal(expectedItems, itemsactualresult);
        }
    }
}

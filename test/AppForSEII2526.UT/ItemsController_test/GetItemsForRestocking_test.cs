using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.Models;
using AppForSEII2526.API.DTOs.ItemDTOs;
using System.Security.Cryptography.Xml;

namespace AppForSEII2526.UT.ItemsController_test
{
    public class GetItemsForRestocking_test : AppForSEII25264SqliteUT
    {
        public GetItemsForRestocking_test()
        {
            var brands = new List<Brand>() {
                new Brand("Brand1"),
                new Brand("Brand2"),
            };

            var typeItems = new List<TypeItem>()
            {
                new TypeItem("Dumbbell"),
                new TypeItem("Machine"),
            };

            var items = new List<Item>()
            {
                new Item("Dumbbell", "Description", 20, 8, 10, 25, brands[0], typeItems[0]),
                new Item("Press machine", "Description2", 300, 9, 10, 200, brands[1], typeItems[1]),
                new Item("Kettlbell", "Description3", 15, 14, 6, 10, brands[0], typeItems[0]),
            };

            _context.AddRange(brands);
            _context.AddRange(typeItems);
            _context.AddRange(items);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetItemsForRestocking_null_name_null_min_null_max()
        {
            var expectedItems = new List<ItemForRestockingDTO>()
            {
                new ItemForRestockingDTO(1, "Dumbbell", "Brand1", "Description", 20, 25, 10, 8),
                new ItemForRestockingDTO(2, "Press Machine", "Brand2", "Description2", 300, 200, 10, 9)
            };
            var controller = new ItemsController(_context, null);

            var result = await controller.GetItemsForRestocking(null, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var itemsDTOsActual = Assert.IsType<List<ItemForRestockingDTO>>(okResult.Value);
            Assert.Equal(expectedItems, itemsDTOsActual);
        }
    }
}

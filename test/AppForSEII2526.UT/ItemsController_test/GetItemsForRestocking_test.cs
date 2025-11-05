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
                //public Item(string name, string description, decimal purchasePrice, int quantityAvailableForPurchase, int quantityForRestock, decimal? restockPrice, Brand brand, TypeItem typeItem)
                //as PurchasePrice attribute is not neccessary nor returned by the method, is initialized to 0
                new Item("Dumbbell", "Description", 0, 8, 10, 25, brands[0], typeItems[0]),
                new Item("Press machine", "Description2", 0, 9, 10, 200, brands[1], typeItems[1]),
                new Item("Kettlbell", "Description3", 0, 14, 6, 10, brands[0], typeItems[0]),
                new Item("MoreDumbell", "Description4", 0, 12, 10, 7, brands[1], typeItems[1])
            };

            _context.AddRange(brands);
            _context.AddRange(typeItems);
            _context.AddRange(items);
            _context.SaveChanges();
        }

        [Theory]
        [Trait ("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetItemsForRestock_OK))]
        public async Task GetItemsForRestocking_null_name_null_min_null_max(string? name, int? min, int? max, List<ItemForRestockingDTO> expectedItems)
        {
            //var expectedItems = new List<ItemForRestockingDTO>()
            //{
            //    //public ItemForRestockingDTO(int id, string name, string brand, string description, decimal purchasePrice, decimal? restockPrice, int quantityForRestock, int quantityAvailableForPurchase) : this(id, name, brand, description)
            //    new ItemForRestockingDTO(1, "Dumbbell", "Brand1", "Description", 0, 25, 10, 8),
            //    new ItemForRestockingDTO(2, "Press machine", "Brand2", "Description2", 0, 200, 10, 9)
            //};
            var controller = new ItemsController(_context, null);

            var result = await controller.GetItemsForRestocking(name, min, max);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var itemsDTOsActual = Assert.IsType<List<ItemForRestockingDTO>>(okResult.Value);
            Assert.Equal(expectedItems, itemsDTOsActual);
        }

        public static IEnumerable<object[]> TestCasesFor_GetItemsForRestock_OK()
        {
            var itemsDTOs = new List<ItemForRestockingDTO> { new ItemForRestockingDTO(1, "Dumbbell", "Brand1", "Description", 0, 25, 10, 8),
                new ItemForRestockingDTO(2, "Press machine", "Brand2", "Description2", 0, 200, 10, 9)};
            var itemDTOsTC1 = new List<ItemForRestockingDTO> { itemsDTOs[0], itemsDTOs[1] };
            var itemDTOsTC2 = new List<ItemForRestockingDTO> { itemsDTOs[0] };
            var itemDTOsTC3 = new List<ItemForRestockingDTO> { itemsDTOs[0] };
            var itemDTOsTC4 = new List<ItemForRestockingDTO> { itemsDTOs[1] };



            var allTests = new List<object[]>
            {
                new object[] {null, null, null, itemDTOsTC1},
                new object[] {"Dumbbell", null, null, itemDTOsTC2},
                new object[] {null, null, 8, itemDTOsTC3},
                new object[] {null, 9, null, itemDTOsTC4}
            };
            return allTests;
        }

        [Fact]
        [Trait ("LevelTesting", "Unit Testing")]
        public async Task GetItemsForRestocking_filter_only_name()
        {
            var expectedItems = new List<ItemForRestockingDTO>()
            {
                //public ItemForRestockingDTO(int id, string name, string brand, string description, decimal purchasePrice, decimal? restockPrice, int quantityForRestock, int quantityAvailableForPurchase) : this(id, name, brand, description)
                new ItemForRestockingDTO(1, "Dumbbell", "Brand1", "Description", 0, 25, 10, 8),
            };

            var controller = new ItemsController(_context, null);

            var result = await controller.GetItemsForRestocking("Dumbbell", null, null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var itemsDTOsActual = Assert.IsType<List<ItemForRestockingDTO>>(okResult.Value);
            Assert.Equal(expectedItems, itemsDTOsActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetItemsForRestocking_filter_only_max()
        {
            var expectedItems = new List<ItemForRestockingDTO>()
            {
                //public ItemForRestockingDTO(int id, string name, string brand, string description, decimal purchasePrice, decimal? restockPrice, int quantityForRestock, int quantityAvailableForPurchase) : this(id, name, brand, description)
                new ItemForRestockingDTO(1, "Dumbbell", "Brand1", "Description", 0, 25, 10, 8),
            };

            var controller = new ItemsController(_context, null);

            var result = await controller.GetItemsForRestocking(null, null, 8);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var itemsDTOsActual = Assert.IsType<List<ItemForRestockingDTO>>(okResult.Value);
            Assert.Equal(expectedItems, itemsDTOsActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetItemsForRestocking_filter_only_min()
        {
            var expectedItems = new List<ItemForRestockingDTO>()
            {
                //public ItemForRestockingDTO(int id, string name, string brand, string description, decimal purchasePrice, decimal? restockPrice, int quantityForRestock, int quantityAvailableForPurchase) : this(id, name, brand, description)
                new ItemForRestockingDTO(2, "Press machine", "Brand2", "Description2", 0, 200, 10, 9)
            };

            var controller = new ItemsController(_context, null);

            var result = await controller.GetItemsForRestocking(null, 9, null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var itemsDTOsActual = Assert.IsType<List<ItemForRestockingDTO>>(okResult.Value);
            Assert.Equal(expectedItems, itemsDTOsActual);
        }
    }
}

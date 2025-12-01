using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.DTOs.RestockDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.RestocksController_test
{
    public class CreateRestock_test : AppForSEII25264SqliteUT
    {
        public CreateRestock_test()
        {
            var brand = new Brand("Precor");
            var typeItem = new TypeItem("Dumbbell");
            var items = new List<Item>
            {
                new Item("Dumbbell", "Regular dumbbell", 0, 4, 6, 30, brand, typeItem),
                new Item("Kettlebell", "Cicular dumbbell", 0, 5, 3, 40, brand, typeItem),
                new Item("Band", "Elastic band", 0, 3, 4, 0, brand, typeItem)
            };

            _context.AddRange(items);
            _context.SaveChanges();

            var user = new ApplicationUser("Jaime", "Domingo", "Jaime");

            var restock = new Restock("A restock", "An address", "A description", new DateTime(), new DateTime(),
                180, new List<RestockItem>(), user);
            restock.RestockItems.Add(new RestockItem(2, new Restock(), items[0]));
            restock.RestockItems.Add(new RestockItem(3, new Restock(), items[1]));

            _context.Add(user);
            _context.Add(restock);
            _context.SaveChanges();
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreateRestock_Error))]
        public async Task CreateRestock_Error_test(RestockForCreateDTO restockDTO, string errorExpected)
        {
            var mock = new Mock<ILogger<RestocksController>>();
            ILogger<RestocksController> looger = mock.Object;
            var controller = new RestocksController(_context, looger);

            var result = await controller.CreateRestock(restockDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorExpected, errorActual);
        }

        [Trait("LevelTesting", "Unit Testing")]
        public static IEnumerable<object[]> TestCasesFor_CreateRestock_Error()
        {
            RestockForCreateDTO restockBadDate = new RestockForCreateDTO("R1", "C1", "Restock for", 
                DateTime.Today.AddDays(-1), new DateTime(),
                new List<ItemForCreateRestockDTO>() { new ItemForCreateRestockDTO(1, 2) }, "Jaime");

            RestockForCreateDTO restockBadTitle = new RestockForCreateDTO("A restock", "An address", "A description", 
                new DateTime(), new DateTime(),
                new List<ItemForCreateRestockDTO>() { new ItemForCreateRestockDTO(1, 2) }, "Jaime");

            RestockForCreateDTO restockBadDescription = new RestockForCreateDTO("R1", "C1", "I want",
                DateTime.Today.AddDays(1), new DateTime(),
                new List<ItemForCreateRestockDTO>() { new ItemForCreateRestockDTO(1, 1) }, "Jaime");

            RestockForCreateDTO restockNoItem = new RestockForCreateDTO("R1", "C1", "Restock for",
                DateTime.Today.AddDays(1), new DateTime(), new List<ItemForCreateRestockDTO>(), "Jaime");

            RestockForCreateDTO restockNoUser = new RestockForCreateDTO("R1", "C1", "Restock for",
                DateTime.Today.AddDays(1), new DateTime(),
                new List<ItemForCreateRestockDTO>() { new ItemForCreateRestockDTO(1, 2) }, "Manolo");

            RestockForCreateDTO restockItemNotFound = new RestockForCreateDTO("R1", "C1", "Restock for",
                DateTime.Today.AddDays(1), new DateTime(),
                new List<ItemForCreateRestockDTO>() {new ItemForCreateRestockDTO(0, 2) }, "Jaime");

            RestockForCreateDTO restockNoQuantity = new RestockForCreateDTO("R1", "C1", "Restock for",
                DateTime.Today.AddDays(1), new DateTime(),
                new List<ItemForCreateRestockDTO>() { new ItemForCreateRestockDTO(1, -2) }, "Jaime");

            RestockForCreateDTO restockBadQuantity = new RestockForCreateDTO("R1", "C1", "Restock for",
                DateTime.Today.AddDays(1), new DateTime(),
                new List<ItemForCreateRestockDTO>() { new ItemForCreateRestockDTO(1, 1) }, "Jaime");

            var allTest = new List<Object[]>
            {
                new object[] { restockBadDate, "Error! The expected date must start later than today." },
                new object[] { restockBadTitle, "Error! There´s already a restock with that title." },
                new object[] { restockBadDescription, "Error! The description must start with: Restock for" },
                new object[] { restockNoItem, "Error! At least one item must be selected for restock." },
                new object[] { restockNoUser, "Error! User name is not registered" },
                new object[] { restockItemNotFound, "The specified item cannot be found." },
                new object[] { restockNoQuantity, "You need to restock at least one item." },
                new object[] { restockBadQuantity, "Error! The total quantity for purchase 4 plus the" +
                        " quantity to restock 1 of item Dumbbell must be bigger than the quantity for restock 6." }
            };

            return allTest;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreateRestock_Success))]
        public async Task CreateRestock_Success_test(RestockForCreateDTO restockDTO, RestockDetailDTO expected)
        {
            var mock = new Mock<ILogger<RestocksController>>();
            ILogger<RestocksController> looger = mock.Object;
            var controller = new RestocksController(_context, looger);

            var result = await controller.CreateRestock(restockDTO);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsType<RestockDetailDTO>(created.Value);

            Assert.Equal(expected, actual);
        }

        [Trait("LevelTesting", "Unit Testing")]
        public static IEnumerable<object[]> TestCasesFor_CreateRestock_Success()
        {
            RestockForCreateDTO restockDTOWithPrice = new RestockForCreateDTO("First", "Any", "Restock for",
                DateTime.Today.AddDays(1), new DateTime(),
                new List<ItemForCreateRestockDTO> { new ItemForCreateRestockDTO(1, 4) }, "Jaime");

            RestockDetailDTO expectedWithPrice = new RestockDetailDTO(2, "First", "Any", "Restock for", DateTime.Today.AddDays(1),
                120.0m, new List<ItemForRestockingDTO> {
                    new ItemForRestockingDTO(1, "Dumbbell", "Precor", "Regular dumbbell", 0, 120.0m, 4, 4, 4) }, "Jaime", "Domingo");

            RestockForCreateDTO restockDTOWithoutPrice = new RestockForCreateDTO("Second", "Any", "Restock for",
                DateTime.Today.AddDays(1), new DateTime(),
                new List<ItemForCreateRestockDTO> { new ItemForCreateRestockDTO(3, 4) }, "Jaime");

            RestockDetailDTO expectedWithoutPrice = new RestockDetailDTO(2, "Second", "Any", "Restock for", DateTime.Today.AddDays(1),
                null, new List<ItemForRestockingDTO> {
                    new ItemForRestockingDTO(3, "Band", "Precor", "Elastic band", 0, 0, 4, 3, 4) }, "Jaime", "Domingo");

            var allTest = new List<Object[]>
            {
                new Object[] { restockDTOWithPrice, expectedWithPrice },
                new Object[] { restockDTOWithoutPrice, expectedWithoutPrice }
            };

            return allTest;
        }
    }
}

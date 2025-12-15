using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium.DevTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.UC_Restock
{
    public class UC_RestockItems_UIT : UC_UIT
    {
        private SelectItemsForRestocking_PO selectItemsForRestocking_PO;
        private CreateRestock_PO createRestock_PO;
        private DetailRestock_PO detailRestock_PO;

        private const string itemName1 = "Resistance Band";
        private const string itemBrand1 = "Rogue Fitness";
        private const string itemStockQuantity1 = "8";
        private const string itemRestockPrice1 = "7";

        private const int Id2 = 9;
        private const string itemName2 = "20 Kg Kettlebell";
        private const string itemBrand2 = "Precor";
        private const string itemStockQuantity2 = "2";
        private const string itemRestockPrice2 = "40";

        private static readonly string tomorrowDate = DateTime.Today.AddDays(1).ToString();

        public UC_RestockItems_UIT(ITestOutputHelper output) : base(output)
        {
            selectItemsForRestocking_PO = new SelectItemsForRestocking_PO(_driver, _output);
            createRestock_PO = new CreateRestock_PO(_driver, _output);
            detailRestock_PO = new DetailRestock_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("jaime@uclm.es", "12$Temporary");
        }

        private void InitialStepsForRestockItems()
        {
            Precondition_perform_login();
            selectItemsForRestocking_PO.WaitForBeingVisible(By.Id("CreateRestock"));
            _driver.FindElement(By.Id("CreateRestock")).Click();
        }

        [Theory]
        [InlineData(itemName1, itemBrand1, itemStockQuantity1, itemRestockPrice1, "Resistance Band", "", "")]
        [InlineData(itemName2, itemBrand2, itemStockQuantity2, itemRestockPrice2, "", "", "2")]
        [InlineData(itemName1, itemBrand1, itemStockQuantity1, itemRestockPrice1, "", "8", "")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC14_3_AF2_UC14_3_4_5_filtering(string itemName, string itemBrand, string itemStockQuantity, string itemRestockPrice,
            string filterName, string filterMin, string filterMax)
        {
            // Arrange
            InitialStepsForRestockItems();
            var expectedItems = new List<string[]> { new string[] { itemName, itemBrand, itemStockQuantity, itemRestockPrice } };

            // Act
            selectItemsForRestocking_PO.SearchItems(filterName, filterMin, filterMax);

            // Assert
            Assert.True(selectItemsForRestocking_PO.CheckListOfItems(expectedItems));
        }

        //[Fact(Skip = "first run dbo.Items.data.UpdateQuantityAvailable.sql, after run dbo.Items.data.RecoverQuantityAvailable.sql")]
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC14_AF1_UC14_12_no_items_to_restock()
        {
            InitialStepsForRestockItems();
            var expectedItems = new List<string[]> { };

            selectItemsForRestocking_PO.SearchItems("", "", "");

            Assert.True(selectItemsForRestocking_PO.CheckListOfItems(expectedItems));
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC14_AF3_UC14_6_RestockNotAvailable()
        {
            InitialStepsForRestockItems();
            selectItemsForRestocking_PO.AddItemToRestockingCart(itemName2);
            selectItemsForRestocking_PO.RemoveItemFromRestockingCArt(Id2);

            Assert.True(selectItemsForRestocking_PO.RestockNotAvailable());
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC14_AF4_UC14_7()
        {
            string title = "A title1";
            string address = "any";
            string description = "Restock for me";
            string date = "13/12/2028";
            int quantity = 10;

            InitialStepsForRestockItems();

            selectItemsForRestocking_PO.AddItemToRestockingCart(itemName2);

            selectItemsForRestocking_PO.PressRestock();

            createRestock_PO.FillInRestockInfo(title, address, description, date);
            createRestock_PO.FillRestockQuantity(Id2, quantity);
            createRestock_PO.PressRestockItems();

            InitialStepsForRestockItems();

            selectItemsForRestocking_PO.AddItemToRestockingCart(itemName2);

            selectItemsForRestocking_PO.PressRestock();

            createRestock_PO.FillInRestockInfo(title, address, description, date);
            createRestock_PO.FillRestockQuantity(Id2, quantity);
            createRestock_PO.PressRestockItems();

            Assert.True(createRestock_PO.CheckValidationError("Error! There´s already a restock with that title"));
        }

        [Theory]
        [InlineData("A title2", "Any", "", "1/12/2025", 10, "Error! The expected date must start later than today")]
        [InlineData("A title3", "Any", "Wrong description", "13/12/2025", 10, "Error! The description must start with: Restock for")]
        [InlineData("A title4", "Any", "", "13/12/2028", 1, "Error! The total quantity for purchase 2 plus the quantity to " +
            "restock 1 of item 20 Kg Kettlebell must be bigger than the quantity for restock 5.")]
        [InlineData("A title4", "Any", "", "13/12/2028", 0, "You need to restock at least one item.")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC14_AF4_UC14_8_9_10_13(string title, string address, string description,
            string date, int quantity, string errorMessage)
        {
            InitialStepsForRestockItems();

            selectItemsForRestocking_PO.AddItemToRestockingCart(itemName2);

            selectItemsForRestocking_PO.PressRestock();

            createRestock_PO.FillInRestockInfo(title, address, description, date);
            createRestock_PO.FillRestockQuantity(Id2, quantity);

            createRestock_PO.PressRestockItems();

            Assert.True(createRestock_PO.CheckValidationError(errorMessage));
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC14_AF5_UC14_11()
        {
            InitialStepsForRestockItems();

            selectItemsForRestocking_PO.AddItemToRestockingCart(itemName2);
            selectItemsForRestocking_PO.AddItemToRestockingCart("16 Kg Kettlebell");

            selectItemsForRestocking_PO.PressRestock();

            createRestock_PO.GoBack();

            selectItemsForRestocking_PO.RemoveItemFromRestockingCArt(7);

            selectItemsForRestocking_PO.PressRestock();

            var expectedItems = new List<string[]> { new string[] { itemName2, itemBrand2, itemStockQuantity2, itemRestockPrice2 } };

            Assert.True(createRestock_PO.CheckListOfItems(expectedItems));
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC14_BasicFlow_UC14_1()
        {
            string title = "My title";
            string address = "any";
            string description = "Restock for doing";
            string name = "Jaime";
            string surname = "Domingo";
            int quantityToRestock = 10;
            int itemPrice = quantityToRestock * int.Parse(itemRestockPrice2);
            string totalPrice = itemPrice.ToString();

                                                                                        // Total price of all units
            List<string[]> expectedItems = new List<string[]> { new string[] { itemName2, itemBrand2, "400", "10" } };

            InitialStepsForRestockItems();

            selectItemsForRestocking_PO.AddItemToRestockingCart(itemName2);

            selectItemsForRestocking_PO.PressRestock();

            createRestock_PO.FillInRestockInfo(title, address, description, tomorrowDate);
            createRestock_PO.FillRestockQuantity(Id2, quantityToRestock);

            createRestock_PO.PressRestockItems();

            Thread.Sleep(4000);

            Assert.True(detailRestock_PO.CheckRestockDetail(title, address, description,
                DateTime.Parse(tomorrowDate), name, surname, totalPrice));

            Assert.True(detailRestock_PO.CheckListOfItems(expectedItems));
        }

        //[Fact(Skip = "first run dbo.Items.data.price0.sql, after run dbo.Items.data.priceRecover.sql")]
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC14_BasicFlow_UC14_2()
        {
            string title = "My title";
            string address = "any";
            string description = "Restock for doing";
            string name = "Jaime";
            string surname = "Domingo";
            int quantityToRestock = 10;
            string totalPrice = "";

            // Total price of all units
            List<string[]> expectedItems = new List<string[]> { new string[] { itemName2, itemBrand2, "0", "10" } };

            InitialStepsForRestockItems();

            selectItemsForRestocking_PO.AddItemToRestockingCart(itemName2);

            selectItemsForRestocking_PO.PressRestock();

            createRestock_PO.FillInRestockInfo(title, address, description, tomorrowDate);
            createRestock_PO.FillRestockQuantity(Id2, quantityToRestock);

            createRestock_PO.PressRestockItems();

            Assert.True(detailRestock_PO.CheckRestockDetail(title, address, description,
                DateTime.Parse(tomorrowDate), name, surname, totalPrice));

            Assert.True(detailRestock_PO.CheckListOfItems(expectedItems));
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC14_BasicFlow_AF2_AF5()
        {
            string title = "My title for new restock for exam";
            string address = "any";
            string description = "Restock for doing";
            string name = "Jaime";
            string surname = "Domingo";
            int quantityToRestock = 10;
            string totalPrice = "400";

            // Total price of all units
            List<string[]> expectedItems = new List<string[]> { new string[] { itemName2, itemBrand2, "400", "10" } };

            InitialStepsForRestockItems();

            selectItemsForRestocking_PO.AddItemToRestockingCart("20 Kg Dumbbell");

            selectItemsForRestocking_PO.SearchItems(itemName2, "", "");

            selectItemsForRestocking_PO.AddItemToRestockingCart(itemName2);

            selectItemsForRestocking_PO.RemoveItemFromRestockingCArt(8);
            selectItemsForRestocking_PO.PressRestock();

            createRestock_PO.FillInRestockInfo(title, address, description, tomorrowDate);
            createRestock_PO.FillRestockQuantity(Id2, quantityToRestock);

            createRestock_PO.PressRestockItems();

            Assert.True(detailRestock_PO.CheckRestockDetail(title, address, description,
                DateTime.Parse(tomorrowDate), name, surname, totalPrice));

            Assert.True(detailRestock_PO.CheckListOfItems(expectedItems));
        }
    }
}

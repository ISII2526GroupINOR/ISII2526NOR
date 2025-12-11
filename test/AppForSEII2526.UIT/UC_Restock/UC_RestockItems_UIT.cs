using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium.DevTools.V140.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Restock
{
    public class UC_RestockItems_UIT : UC_UIT
    {
        private SelectItemsForRestocking_PO selectItemsForRestocking_PO;
        private CreateRestock_PO createRestock_PO;
        private const string itemName1 = "Resistance Band";
        private const string itemBrand1 = "Rogue Fitness";
        private const string itemStockQuantity1 = "8";
        private const string itemRestockPrice1 = "7";

        private const int Id2 = 9;
        private const string itemName2 = "20 Kg Kettlebell";
        private const string itemBrand2 = "Precor";
        private const string itemStockQuantity2 = "2";
        private const string itemRestockPrice2 = "40";

        public UC_RestockItems_UIT(ITestOutputHelper output) : base(output)
        {
            selectItemsForRestocking_PO = new SelectItemsForRestocking_PO(_driver, _output);
            createRestock_PO = new CreateRestock_PO(_driver, _output);
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
        public void UC14_3_AF2_UC14_2_3_4_filtering(string itemName, string itemBrand, string itemStockQuantity, string itemRestockPrice,
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

        //[Fact(Skip = "first run dbo.Items.data.UpdateQuantityAvailable.sql, after run dbo.Items.data.RecoverQuantityAvailable")]
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC14_AF1_UC14_13_filtering()
        {
            InitialStepsForRestockItems();
            var expectedItems = new List<string[]> { };

            selectItemsForRestocking_PO.SearchItems("", "", "");

            Assert.True(selectItemsForRestocking_PO.CheckListOfItems(expectedItems));
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC14_AF3_UC14_5_RestockNotAvailable()
        {
            InitialStepsForRestockItems();
            selectItemsForRestocking_PO.AddItemToRestockingCart(itemName2);
            selectItemsForRestocking_PO.RemoveItemFromRestockingCArt(Id2);

            Assert.True(selectItemsForRestocking_PO.RestockNotAvailable());
        }
    }
}

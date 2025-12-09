using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium.DevTools.V140.Network;
using Xunit;

namespace AppForSEII2526.UIT.UC_PurchaseItems
{
    public class UC_PurchaseItems_UIT : UC_UIT
    {
        private SelectItemsForPurchase_PO selectItemsForPurchase_PO;

        private const int itemId1 = 1;
        private const string itemName1 = "10 Kg Dumbbell";
        private const string itemBrand1 = "Rogue Fitness";
        private const string itemDescription1 = "A dumbbell";
        private const string itemPrice1 = "20€";
        private const string itemQuantityAvailable1 = "10";



        private const int itemId2 = 2;
        private const string itemName2 = "15 Kg Dumbbell";
        private const string itemBrand2 = "Precor";
        private const string itemDescription2 = "A regular dumbbell";
        private const string itemPrice2 = "25€";
        private const string itemQuantityAvailable2 = "16";


        public UC_PurchaseItems_UIT(ITestOutputHelper output) : base(output)
        {
            selectItemsForPurchase_PO = new SelectItemsForPurchase_PO(_driver, _output);


        }

        private void Precondition_perform_login()
        {
            Perform_login("sergio@uclm.es", "01234Pwd!");
        }


        private void InitialStepsForPurchaseItems()
        {
            Precondition_perform_login();

            selectItemsForPurchase_PO.WaitForBeingVisible(By.Id("CreatePurchase"));

            _driver.FindElement(By.Id("CreatePurchase")).Click();
        }

        [Theory]
        [InlineData(itemName1, itemBrand1, itemDescription1, itemPrice1, itemQuantityAvailable1, "10 Kg D", "")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC2_AF1_UC2_4_5_6_filtering(string itemName, string itemBrand, string itemDescription, string itemPrice, string itemQuantityAvailable, string filterName, string filterBrand)
        {
            InitialStepsForPurchaseItems();
            var expectedItems = new List<string[]>
            {
                new string[] {
                    itemName, itemBrand, itemDescription, itemPrice,
                    itemQuantityAvailable
                }
            };


            selectItemsForPurchase_PO.searchItems(filterName, filterBrand);


            Assert.True(selectItemsForPurchase_PO.checkListOfItems(expectedItems));
        }

        //itemToPurchase_10 Kg Dumbbell


    }
}

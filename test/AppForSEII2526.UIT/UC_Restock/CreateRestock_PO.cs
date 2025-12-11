using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Restock
{
    public class CreateRestock_PO : PageObject
    {
        By restockTitleBy = By.Name("restock.Title");
        By restockAddressBy = By.Name("restock.DeliveryAddress");
        By restockDescriptionBy = By.Name("restock.Description");
        By restockDateBy = By.Name("restock.ExpectedDate");
        By tableOfItemsBy = By.Id("TableOfRestockItems");
        By errorBox = By.Id("ErrorShown");
        IWebElement restockTitle() => _driver.FindElement(restockTitleBy);
        IWebElement restockAddress() => _driver.FindElement(restockAddressBy);
        IWebElement restockDescription() => _driver.FindElement(restockDescriptionBy);
        IWebElement restockDate() => _driver.FindElement(restockDateBy);

        public CreateRestock_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillInRestockInfo(string title,string address, string description, string date)
        {
            WaitForBeingVisible(restockTitleBy);
            restockTitle().SendKeys(title);

            WaitForBeingClickable(restockAddressBy);
            restockAddress().SendKeys(address);

            WaitForBeingClickable(restockDescriptionBy);
            restockDescription().SendKeys(description);

            WaitForBeingVisible(restockDateBy);
            restockDate().SendKeys(date);
        }

        public void FillRestockQuantity(int itemId, int quantity)
        {
            _driver.FindElement(By.Id("Quantity_" +  itemId)).SendKeys(quantity.ToString());
        }

        public void PressRestockItems()
        {
            _driver.FindElement(By.Id("submit")).Click();
            WaitForBeingClickable(By.Id("Button_DialogOK"));
            _driver.FindElement(By.Id("Button_DialogOK")).Click();
        }

        public bool CheckListOfItems(List<string[]> expectedItems)
        {
            return CheckBodyTable(expectedItems, tableOfItemsBy);
        }

        public bool CheckValidationError(string errorMessage)
        {
            WaitForBeingVisible(errorBox);
            return _driver.FindElement(errorBox).Text.Contains(errorMessage);
        }

        public void GoBack()
        {
            WaitForBeingClickable(By.Id("ModifyItems"));

            _driver.FindElement(By.Id("ModifyItems")).Click();
        }
    }
}

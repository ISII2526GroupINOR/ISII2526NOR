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
        IWebElement restockTitle() => _driver.FindElement(restockTitleBy);
        IWebElement restockAddress() => _driver.FindElement(restockAddressBy);
        IWebElement restockDescription() => _driver.FindElement(restockDescriptionBy);
        IWebElement restockDate() => _driver.FindElement(restockDateBy);

        public CreateRestock_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillInRestockInfo(string title,string address, string description, DateTime date)
        {
            WaitForBeingVisible(restockTitleBy);
            restockTitle().SendKeys(title);

            WaitForBeingClickable(restockAddressBy);
            restockAddress().SendKeys(address);

            WaitForBeingClickable(restockDescriptionBy);
            restockDescription().SendKeys(description);

            WaitForBeingVisible(restockDateBy);
            restockDate().SendKeys(date.ToString());
        }

        public void FillRestockQuantity(int itemId, int quantity)
        {
            _driver.FindElement(By.Id("Quantity_" +  itemId)).SendKeys(quantity.ToString());
        }

        public void PressRestockItems()
        {
            _driver.FindElement(By.Id("submit")).Click();
        }
    }
}

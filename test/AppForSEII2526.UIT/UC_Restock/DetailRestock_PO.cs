using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Restock
{
    public class DetailRestock_PO : PageObject
    {
        By restockNameBy = By.Id("Name");
        By restockSurnameBy = By.Id("Surname");
        By restockTitleBy = By.Id("Title");
        By restockAddressBy = By.Id("DeliveryAddress");
        By restockDescriptionBy = By.Id("Description");
        By restockExpectedDateBy = By.Id("ExpectedDate");
        By restockTotalPriceBy = By.Id("TotalPrice");

        public DetailRestock_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckRestockDetail(string title,string address, string description,
            DateTime expectedDate,string name, string surname, string totalPrice)
        {
            WaitForBeingVisible(restockNameBy);

            bool result = true;
            result = result && _driver.FindElement(restockNameBy).Text.Contains(name);
            result = result && _driver.FindElement(restockSurnameBy).Text.Contains(surname);
            result = result && _driver.FindElement(restockTitleBy).Text.Contains(title);
            result = result && _driver.FindElement(restockAddressBy).Text.Contains(address);
            result = result && _driver.FindElement(restockDescriptionBy).Text.Contains(description);
            if (totalPrice == "") result = result && !_driver.FindElements(restockTotalPriceBy).Any();
            else result = result && _driver.FindElement(restockTotalPriceBy).Text.Contains(totalPrice);

            var actualExpectedDate = DateTime.Parse(_driver.FindElement(restockExpectedDateBy).Text);

            result = result && actualExpectedDate.Date == expectedDate.Date; // only comparing dates, no hours

            return result;
        }

        public bool CheckListOfItems(List<string[]> expectedItems)
        {
            return CheckBodyTable(expectedItems, By.Id("RestockedItems"));
        }
    }
}

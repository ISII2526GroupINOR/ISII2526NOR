using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_PurchaseItems
{
    public class SelectItemsForPurchase_PO : PageObject
    {
        By inputName = By.Id("inputName");
        By inputBrand = By.Id("inputBrand");
        By buttonSearchItems = By.Id("searchItems");
        By tableOfItemsBy = By.Id("TableOfItems");
        

        public SelectItemsForPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void searchItems(string name, string brand)
        {
            WaitForBeingClickable(inputName);
            _driver.FindElement(inputName).SendKeys(name);

            WaitForBeingClickable(inputBrand);
            _driver.FindElement(inputBrand).SendKeys(brand);

            _driver.FindElement(buttonSearchItems).Click();
        }

        public bool checkListOfItems(List<string[]> expectedItems)
        {
            return CheckBodyTable(expectedItems, tableOfItemsBy);
        }
    }
}

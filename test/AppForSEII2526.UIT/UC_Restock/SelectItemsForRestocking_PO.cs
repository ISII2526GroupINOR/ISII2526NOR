using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Restock
{
    public class SelectItemsForRestocking_PO : PageObject
    {
        By inputName = By.Id("inputName");
        By inputmin = By.Id("inputMin");
        By inputmax = By.Id("inputMax");
        By buttonSearchItems = By.Id("searchItems");

        public SelectItemsForRestocking_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchItems(string itemName)
        {
            WaitForBeingClickable(inputName);
            _driver.FindElement(inputName).SendKeys(itemName);
            _driver.FindElement(buttonSearchItems).Click();
        }
    }
}

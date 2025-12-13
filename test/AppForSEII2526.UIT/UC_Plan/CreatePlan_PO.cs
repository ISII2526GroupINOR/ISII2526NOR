using OpenQA.Selenium.BiDi.BrowsingContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class CreatePlan_PO : PageObject
    {
        By inputName = By.Id("planName");
        By inputDescription = By.Id("planDescription");
        By inputWeeks = By.Id("planWeeks");
        By inputHealthIssues = By.Id("planHealth");
        By buttonSubmit = By.Id("Submit");
        By buttonModifyClasses = By.Id("buttonModifyClasses");
        By tableSelectedClasses = By.Id("tableOfPlanClasses");
        By errorPanel = By.Id("errorsShown");

        public CreatePlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public bool CheckListOfItems(List<string[]> expectedItems)
        {
            return CheckBodyTable(expectedItems, tableSelectedClasses);
        }

        public string GetErrorMessage()
        {
            try
            {
                WaitForBeingVisible(errorPanel);
                string actualMessage = _driver.FindElement(errorPanel).Text;

                return actualMessage;
            }
            catch (WebDriverTimeoutException)
            {
                return "";
            }
        }
    }
}

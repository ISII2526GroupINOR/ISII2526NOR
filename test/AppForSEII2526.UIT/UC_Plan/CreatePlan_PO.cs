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

        public void ModifyClasses()
        {
            WaitForBeingClickable(buttonModifyClasses);
            _driver.FindElement(buttonModifyClasses).Click();
        }

        public void setPlanName(string name)
        {
            WaitForBeingClickable(inputName);
            _driver.FindElement(inputName).Clear();
            _driver.FindElement(inputName).SendKeys(name);
        }

        public void setPlanDescription(string description)
        {
            WaitForBeingClickable(inputDescription);
            _driver.FindElement(inputDescription).Clear();
            _driver.FindElement(inputDescription).SendKeys(description);
        }

        public void setPlanWeeks(int weeks)
        {
            WaitForBeingClickable(inputWeeks);
            _driver.FindElement(inputWeeks).Clear();
            _driver.FindElement(inputWeeks).SendKeys(weeks.ToString());
        }

        public void setPlanHealthIssues(string healthIssues)
        {
            WaitForBeingClickable(inputHealthIssues);
            _driver.FindElement(inputHealthIssues).Clear();
            _driver.FindElement(inputHealthIssues).SendKeys(healthIssues);
        }

        // TODO: implement method to set the payment method when it is available

        public void SubmitPlan()
        {
            WaitForBeingClickable(buttonSubmit);
            _driver.FindElement(buttonSubmit).Click();
        }

        public string GetErrors()
        {
            WaitForBeingVisible(errorPanel);
            return _driver.FindElement(errorPanel).Text;
        }
    }
}

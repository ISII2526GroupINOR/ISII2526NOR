using OpenQA.Selenium.BiDi.BrowsingContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class SelectClassesForPlan_PO : PageObject
    {
        By inputName = By.Id("inputName");
        By inputDate = By.Id("inputDate");
        By button = By.Id("searcClasses");

        public SelectClassesForPlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
            
            
        }

        public void SearchClasses(string className)
        {
            // Wait for the webelement to be clickable
            WaitForBeingClickable(inputName);

            _driver.FindElement(inputName).SendKeys(className);
            _driver.FindElement(button).Click();
            

        }
    }
}

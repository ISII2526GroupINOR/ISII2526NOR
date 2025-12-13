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
        By button = By.Id("searchClasses");
        By tableOfClassesBy = By.Id("tableOfClasses");
        By createPlanButton = By.Id("purchaseClassesButton");

        public SelectClassesForPlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public void SearchClasses(string className, DateOnly? classDate)
        {
            // Write the className into the inputName webelement
            WaitForBeingClickable(inputName);
            _driver.FindElement(inputName).SendKeys(className);

            // Write the classDate into the inputDate webelement
            // If classDate is null, do not write anything. This allows searching without date filter on some test cases.
            if (classDate != null)
            {
                DateOnly strictClassDate = (DateOnly)classDate;
                WaitForBeingClickable(inputDate);
                _driver.FindElement(inputDate).SendKeys(strictClassDate.ToString("dd/MM/yyyy"));
            }



            // Perform search by clicking the button
            WaitForBeingClickable(button);
            _driver.FindElement(button).Click();

        }


        public bool CheckListOfClasses(List<string[]> expectedClasses)
        {
            return CheckBodyTable(expectedClasses, tableOfClassesBy);
        }


        public string GetErrorMessage()
        {
            By errorPanel = By.Id("errorsShown");

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

        public string GetWarningMessage()
        {
            By warningPanel = By.Id("warningsShown");

            try
            {
                WaitForBeingVisible(warningPanel);
                string actualMessage = _driver.FindElement(warningPanel).Text;

                return actualMessage;
            }
            catch (WebDriverTimeoutException)
            {
                return "";
            }
        }

        public void PressCreatePlanButton()
        {

            WaitForBeingClickable(createPlanButton);
            _driver.FindElement(createPlanButton).Click();
        }

        public void AddClassToSelected(string className)
        {
            By addButton = By.Id($"bt_add_class_{className}");
            WaitForBeingClickable(addButton);
            _driver.FindElement(addButton).Click();
        }

        public void RemoveClassFromSelected(string className)
        {
            By removeButton = By.Id($"removeClass_{className}");
            WaitForBeingClickable(removeButton);
            _driver.FindElement(removeButton).Click();
        }


        
        public bool IsCreatePlanButtonVisible()
        {
            try
            {
                // Only wait for 2 seconds to make the test faster, while taking into account big latencies in the response time of the web application
                WaitForBeingVisibleVaryingTime(createPlanButton, 2);
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }

        }
    }
  
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class UC_PlanClasses_UIT : UC_UIT
    {
        private SelectClassesForPlan_PO selectClassesForPlan_PO;

        public UC_PlanClasses_UIT(ITestOutputHelper output) : base(output)
        {
            
        }


        private void Precondition_perform_login()
        {
            Perform_login("user2@email.lan", "Pass1234$"); // Plain text credentials in version control are intended for testing purposes.
        }


        private void InitialStepsForPlanClasses()
        {
            Precondition_perform_login();

            // Wait for the option of the menu to be visible
            selectClassesForPlan_PO.WaitForBeingVisible(By.Id("CreatePlan"));

            // Click on the option of the menu to go to the plan creation page
            _driver.FindElement(By.Id("CreatePlan")).Click();   
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.AuxiliaryClasses;
using System.Globalization; // For CultureInfo.InvariantCulture, because spanish windows forces commas as decimal separator everywhere

namespace AppForSEII2526.UIT.UC_Plan
{
    public class UC_PlanClasses_UIT : UC_UIT
    {
        private SelectClassesForPlan_PO selectClassesForPlan_PO;
        // Class 1
        private const int classId1 = 1;
        private const string className1 = "Judo";
        private readonly DateOnly classDate1 = TimeTable.nextWeekMonday;
        private readonly TimeOnly classTime1 = TimeTable.timeMorning;
        private const decimal classPrice1 = 250.00m;
        private readonly List<string> classTypeItems1 = new List<string> {"Mat"};
        // Class 2
        private const int classId2 = 2;
        private const string className2 = "Boxing";
        private readonly DateOnly classDate2 = TimeTable.middleOfNextWeek;
        private readonly TimeOnly classTime2 = TimeTable.timeAfternoon;
        private const decimal classPrice2 = 400.00m;
        private readonly List<string> classTypeItems2 = new List<string> {"Gloves", "Punching Bag"};
        // Class 3
        private const int classId3 = 3;
        private const string className3 = "Yoga";
        private readonly DateOnly classDate3 = TimeTable.nextWeekSunday;
        private readonly TimeOnly classTime3 = TimeTable.timeEvening;
        private const decimal classPrice3 = 100.00m;
        private readonly List<string> classTypeItems3 = new List<string> {"Machine"};


        public UC_PlanClasses_UIT(ITestOutputHelper output) : base(output)
        {
            selectClassesForPlan_PO = new SelectClassesForPlan_PO(_driver, _output);
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


        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_ES1_1_createAPlan()
        {
            // ARRANGE
            InitialStepsForPlanClasses();

            var expectedClasses = new List<string[]> // Use default decimal places for prices
            {
                new string[] { className1, classDate1.ToString("dd/MM/yyyy"), classTime1.ToString("HH:mm"), classPrice1.ToString("F2", CultureInfo.InvariantCulture), string.Join(", ", classTypeItems1), "Add" },
                new string[] { className2, classDate2.ToString("dd/MM/yyyy"), classTime2.ToString("HH:mm"), classPrice2.ToString("F2", CultureInfo.InvariantCulture), string.Join(", ", classTypeItems2), "Add" },
                new string[] { className3, classDate3.ToString("dd/MM/yyyy"), classTime3.ToString("HH:mm"), classPrice3.ToString("F2", CultureInfo.InvariantCulture), string.Join(", ", classTypeItems3), "Add" }
            };

            // ACT
            selectClassesForPlan_PO.SearchClasses("", null); // No filters

            // ASSERT
            Assert.True(selectClassesForPlan_PO.CheckListOfClasses(expectedClasses));
        }
    }
}

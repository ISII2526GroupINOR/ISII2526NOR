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
        private static readonly DateOnly classDate1 = TimeTable.nextWeekMonday;
        private static readonly TimeOnly classTime1 = TimeTable.timeMorning;
        private const decimal classPrice1 = 250.00m;
        private static readonly List<string> classTypeItems1 = new List<string> { "Mat" };
        // Class 2
        private const int classId2 = 2;
        private const string className2 = "Boxing";
        private static readonly DateOnly classDate2 = TimeTable.middleOfNextWeek;
        private static readonly TimeOnly classTime2 = TimeTable.timeAfternoon;
        private const decimal classPrice2 = 400.00m;
        private static readonly List<string> classTypeItems2 = new List<string> { "Gloves", "Punching Bag" };
        // Class 3
        private const int classId3 = 3;
        private const string className3 = "Yoga";
        private static readonly DateOnly classDate3 = TimeTable.nextWeekSunday;
        private static readonly TimeOnly classTime3 = TimeTable.timeEvening;
        private const decimal classPrice3 = 100.00m;
        private static readonly List<string> classTypeItems3 = new List<string> { "Machine" };


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




        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [MemberData(nameof(TestCasesFor_UC1ES1P1_and_UC1ES3P1_and_UC1ES3P2))]
        public void UC1ES1P1_and_UC1ES3P1_and_UC1ES3P2(string classNameFilter, DateOnly? classDateFilter, List<string[]> expectedClasses)
        {
            // ARRANGE
            InitialStepsForPlanClasses();
            // ACT
            selectClassesForPlan_PO.SearchClasses(classNameFilter, classDateFilter);
            // ASSERT
            Assert.True(selectClassesForPlan_PO.CheckListOfClasses(expectedClasses));
        }

        public static IEnumerable<object[]> TestCasesFor_UC1ES1P1_and_UC1ES3P1_and_UC1ES3P2()
        {
            // Prepare expected classes data for test cases UC1_ES1_1 and UC1_ES3_1 to UC1_ES3_2
            var expectedClasses_UC1ES1P1 = new List<string[]> // No filters
            {
                new string[] { className1, classDate1.ToString("dd/MM/yyyy"), classTime1.ToString("HH:mm"), classPrice1.ToString("F2", CultureInfo.InvariantCulture), string.Join(", ", classTypeItems1), "Add" },
                new string[] { className2, classDate2.ToString("dd/MM/yyyy"), classTime2.ToString("HH:mm"), classPrice2.ToString("F2", CultureInfo.InvariantCulture), string.Join(", ", classTypeItems2), "Add" },
                new string[] { className3, classDate3.ToString("dd/MM/yyyy"), classTime3.ToString("HH:mm"), classPrice3.ToString("F2", CultureInfo.InvariantCulture), string.Join(", ", classTypeItems3), "Add" }
            };
            var expectedClasses_UC1ES3P1 = new List<string[]> // Filter "Ju"
            {
                new string[] { className1, classDate1.ToString("dd/MM/yyyy"), classTime1.ToString("HH:mm"), classPrice1.ToString("F2", CultureInfo.InvariantCulture), string.Join(", ", classTypeItems1), "Add" },
            };
            var expectedClasses_UC1ES3P2 = new List<string[]> //Filter by date 
            {
                new string[] { className2, classDate2.ToString("dd/MM/yyyy"), classTime2.ToString("HH:mm"), classPrice2.ToString("F2", CultureInfo.InvariantCulture), string.Join(", ", classTypeItems2), "Add" },
            };

            // Prepare test cases
            var testCases = new List<object[]>
            {
                new object[] { "", null, expectedClasses_UC1ES1P1 }, // UC1_ES1_1: No filters
                new object[] { "Ju", null, expectedClasses_UC1ES3P1 }, // UC1_ES3_1: Filter by name "Ju"
                new object[] { "", TimeTable.middleOfNextWeek, expectedClasses_UC1ES3P2 } // UC1_ES3_2: Filter by date classDate2
            };


            return testCases;
        }



        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1ES2P1()
        {
            // ARRANGE
            InitialStepsForPlanClasses();
            var expectedErrorMessage = "Errors: The class date must be within the next week (from Monday to Sunday)";

            // ACT
            selectClassesForPlan_PO.SearchClasses("", TimeTable.followingWeekMonday);

            // ASSERT
            Assert.Contains(expectedErrorMessage, selectClassesForPlan_PO.GetErrorMessage());
        }



        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1ES4P1()
        {
            // ARRANGE
            InitialStepsForPlanClasses();
            var expectedWarningMessage = "Warnings: No classes were found with the given filters";

            // ACT
            selectClassesForPlan_PO.SearchClasses("NonExistentClassName", null);

            // ASSERT
            Assert.Contains(expectedWarningMessage, selectClassesForPlan_PO.GetWarningMessage());
        }
    }
}

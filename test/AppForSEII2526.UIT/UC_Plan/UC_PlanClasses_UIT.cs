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
        private CreatePlan_PO createPlan_PO;
        private DetailPlan_PO detailPlan_PO;
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
            createPlan_PO = new CreatePlan_PO(_driver, _output);
            detailPlan_PO = new DetailPlan_PO(_driver, _output);
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


        /************************************************
         *  TEST CASES FOR: NON-ALTERNATIVE FLOW        *
         ************************************************/

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1ES1P1_normal_flow()
        {
            // ARRANGE
            InitialStepsForPlanClasses();
            string expectedPlanName = "UIT-PlanName";
            string? expectedPlanDescription = "UIT-PlanDescription";
            int expectedPlanWeeks = 1;
            string? expectedHealthIssues = "UIT-HealthIssues";

            var expectedClasses = new List<string[]>
            {
                new string[] { className1, string.Join(", ", classTypeItems1), classPrice1.ToString("F2", CultureInfo.InvariantCulture), classDate1.ToString("dd/MM/yyyy"), classTime1.ToString("HH:mm"), "" },
                new string[] { className2, string.Join(", ", classTypeItems2), classPrice2.ToString("F2", CultureInfo.InvariantCulture), classDate2.ToString("dd/MM/yyyy"), classTime2.ToString("HH:mm"), "" },
            };
            // ACT

            // Select classes 1 and 2
            selectClassesForPlan_PO.SearchClasses("", null);
            selectClassesForPlan_PO.AddClassToSelected(className1);
            selectClassesForPlan_PO.AddClassToSelected(className2);
            selectClassesForPlan_PO.PressCreatePlanButton();

            // Fill plan information
            createPlan_PO.setPlanName(expectedPlanName);
            createPlan_PO.setPlanDescription(expectedPlanDescription);
            createPlan_PO.setPlanWeeks(expectedPlanWeeks);
            createPlan_PO.setPlanHealthIssues(expectedHealthIssues);
            createPlan_PO.SubmitPlan();
            createPlan_PO.ConfirmPlanSubmission();


            // ASSERT

            // Check that the plan details are correct
            Assert.True(detailPlan_PO.CheckPlanDetail("user2Name user2Surname", TimeTable.today.ToShortDateString(), expectedPlanName, expectedPlanDescription, expectedPlanWeeks.ToString(), expectedHealthIssues));
        }


        /************************************************
         *  TEST CASES FOR: SELECT CLASSES FOR PLAN     *
         ************************************************/

        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [MemberData(nameof(TestCasesFor_UC1ES1P1_and_UC1ES3P1_and_UC1ES3P2))]
        public void UC1ES3P1_and_UC1ES3P2_filters(string classNameFilter, DateOnly? classDateFilter, List<string[]> expectedClasses)
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
        public void UC1ES4P1_invalid_date()
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
        public void UC1ES2P1_no_classes_available()
        {
            // ARRANGE
            InitialStepsForPlanClasses();
            var expectedWarningMessage = "Warnings: No classes were found with the given filters";

            // ACT
            selectClassesForPlan_PO.SearchClasses("NonExistentClassName", null);

            // ASSERT
            Assert.Contains(expectedWarningMessage, selectClassesForPlan_PO.GetWarningMessage());
        }



        /************************************************
         *  TEST CASES FOR: CREATE PLAN                 *
         ************************************************/
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1ES5P1_modify()
        {
            // ARRANGE
            InitialStepsForPlanClasses();

            var expectedclasses1 = new List<string[]>
            {
                new string[] { className1, classDate1.ToString("dd/MM/yyyy"), classTime1.ToString("HH:mm"), string.Join(", ", classTypeItems1), classPrice1.ToString("F2", CultureInfo.InvariantCulture) },
                new string[] { className2, classDate2.ToString("dd/MM/yyyy"), classTime2.ToString("HH:mm"), string.Join(", ", classTypeItems2), classPrice2.ToString("F2", CultureInfo.InvariantCulture) },
            };

            var expectedclasses2 = new List<string[]>
            {
                new string[] { className2, classDate2.ToString("dd/MM/yyyy"), classTime2.ToString("HH:mm"), string.Join(", ", classTypeItems2), classPrice2.ToString("F2", CultureInfo.InvariantCulture) },
            };

            // ACT

            // Select classes 1 and 2
            selectClassesForPlan_PO.SearchClasses("", null);
            selectClassesForPlan_PO.AddClassToSelected(className1);
            selectClassesForPlan_PO.AddClassToSelected(className2);
            // Go to create plan page
            selectClassesForPlan_PO.PressCreatePlanButton();
            // Get classes shown in create plan page
            bool check1 = createPlan_PO.CheckListOfItems(expectedclasses1);
            // Go back to select classes page
            createPlan_PO.ModifyClasses();
            // Remove class 2 from selected
            selectClassesForPlan_PO.RemoveClassFromSelected(className1);
            // Go to create plan page again
            selectClassesForPlan_PO.PressCreatePlanButton();
            // Get classes shown in create plan page
            bool check2 = createPlan_PO.CheckListOfItems(expectedclasses2);

            // ASSERT
            Assert.True(check1);
            Assert.True(check2);
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1ES6P1_no_classes_selected()
        {
            /*
             * Note:  Although this test case belongs to step 7 of the use case, it is included in the class selection part (step 4)
             * because the user cannot access step 7 when the alternative flow associated with UC1ES6P1 is taken.
             * 
             * Note: This test case is intended to last around 2 or more seconds.
             * The fact that the duration of the test case is longer than usual does not imply the test has failed.
             */

            // ARRANGE
            InitialStepsForPlanClasses();

            // ACT
            selectClassesForPlan_PO.SearchClasses("", null);

            // ASSERT
            Assert.False(selectClassesForPlan_PO.IsCreatePlanButtonVisible());
        }

        /************************************************
        *  TEST CASES FOR: PLAN DETAIL                  *
        ************************************************/

        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [MemberData(nameof(TestCasesFor_UC1ES7P1_to_UC1ES7P3))]
        public void UC1ES7P1_to_UC1ES7P3_missing_mandatory_plan_information(string? planName, string? planDescription, int? planWeeks, string? healthIssues, By panelToTest, string expectedErrorMessage)
        {
            // ARRANGE
            InitialStepsForPlanClasses();

            // ACT

            // Select classes 1 and 2
            selectClassesForPlan_PO.SearchClasses("", null);
            selectClassesForPlan_PO.AddClassToSelected(className1);
            selectClassesForPlan_PO.AddClassToSelected(className2);
            selectClassesForPlan_PO.PressCreatePlanButton();
            // Fill plan information with the fields provided in the test case
            if (planName != null)
            {
                createPlan_PO.setPlanName(planName);
            }
            if (planDescription != null)
            {
                createPlan_PO.setPlanDescription(planDescription);
            }
            if (planWeeks != null)
            {
                createPlan_PO.setPlanWeeks((int)planWeeks);
            }
            if (healthIssues != null)
            {
                createPlan_PO.setPlanHealthIssues(healthIssues);
            }
            // Submit the plan creation form
            createPlan_PO.SubmitPlan();

            // ASSERT

            // Get the errors
            string actualErrors = createPlan_PO.GetValidations(panelToTest);
            Assert.Contains(expectedErrorMessage, actualErrors);
        }

        public static IEnumerable<object[]> TestCasesFor_UC1ES7P1_to_UC1ES7P3()
        {
            // Prepare test cases
            var testCases = new List<object[]>
            {
                new object[] { null, "UIT_description", 1, "UIT_healthIssues", By.Id("validationName"),  "The Name field is required." },
                new object[] {"UIT_plan", "UIT_description", null, "UIT_healthIssues", By.Id("validationWeeks"), "The field Weeks must be between 1 and 52." }
                // TODO: Implement the test case for UC1ES7P3 for the payment method.

            };
            return testCases;
        }


        /************************************************
         *  TEST CASES FOR: EXAM                        *
         ************************************************/

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1ES9P1_exam()
        {
            // --- ARRANGE ---
            InitialStepsForPlanClasses();
            string expectedPlanName = "UIT-ExamPlanName";
            string? expectedPlanDescription = "UIT-ExamPlanDescription";
            int expectedPlanWeeks = 1;
            string? expectedHealthIssues = "UIT-HealthIssues";


            var expectedFilteredClasses = new List<string[]>
            {
                new string[] { className2, classDate2.ToString("dd/MM/yyyy"), classTime2.ToString("HH:mm"), classPrice2.ToString("F2", CultureInfo.InvariantCulture), string.Join(", ", classTypeItems2), "Add" },
                new string[] { className3, classDate3.ToString("dd/MM/yyyy"), classTime3.ToString("HH:mm"), classPrice3.ToString("F2", CultureInfo.InvariantCulture), string.Join(", ", classTypeItems3), "Add" }
            };

            var expectedClassesDetail = new List<string[]>
            {
                new string[] { className3, string.Join(", ", classTypeItems3), classPrice3.ToString("F2", CultureInfo.InvariantCulture), classDate3.ToString("dd/MM/yyyy"), classTime3.ToString("HH:mm")}
            };

            // --- ACT ---

            // Select classes 1 and 2
            selectClassesForPlan_PO.SearchClasses("", null);
            selectClassesForPlan_PO.AddClassToSelected(className2); // Add first class            

            selectClassesForPlan_PO.SearchClasses("g", null); // filter by type

            // --- INTERMEDIATE ASSERT ---
            Assert.True(selectClassesForPlan_PO.CheckListOfClasses(expectedFilteredClasses)); // This asertions tests the filter

            // --- ACT AGAIN ---

            //select classes
            selectClassesForPlan_PO.AddClassToSelected(className3); // Add second class

            selectClassesForPlan_PO.PressCreatePlanButton();

            // Modify selection
            
            createPlan_PO.ModifyClasses(); // Go back to select classes page
            selectClassesForPlan_PO.RemoveClassFromSelected(className2); // Remove class 2 from selected
            selectClassesForPlan_PO.PressCreatePlanButton(); // Go to create plan page again

            // Fill plan information
            createPlan_PO.setPlanName(expectedPlanName);
            createPlan_PO.setPlanDescription(expectedPlanDescription);
            createPlan_PO.setPlanWeeks(expectedPlanWeeks);
            createPlan_PO.setPlanHealthIssues(expectedHealthIssues);
            createPlan_PO.SubmitPlan();
            createPlan_PO.ConfirmPlanSubmission();


            // --- ASSERT ---

            // Check that the plan details are correct
            Assert.True(detailPlan_PO.CheckPlanDetail("user2Name user2Surname", TimeTable.today.ToShortDateString(), expectedPlanName, expectedPlanDescription, expectedPlanWeeks.ToString(), expectedHealthIssues));

            // Check that the classes are correct
            Assert.True(detailPlan_PO.CheckListOfItems(expectedClassesDetail));
            // This assertion tests the class modification and filtering behavior indirectly
        }
    }
}

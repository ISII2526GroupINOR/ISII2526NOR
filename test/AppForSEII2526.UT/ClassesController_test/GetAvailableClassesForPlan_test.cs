using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForMovies.UT;

using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassDTOs;
using AppForSEII2526.API.Models;
using Microsoft.VisualBasic;
using AppForSEII2526.UT.AuxiliaryClasses; // For TimeTable

namespace AppForSEII2526.UT.ClassesController_test
{
    public class GetAvailableClassesForPlan_test : AppForSEII25264SqliteUT
    {
        


        public GetAvailableClassesForPlan_test()
        {
            // Create objects in the database context (physically stored in main memory for testing)

            var typeItems = new List<TypeItem>
            {
                new TypeItem("Bench"),
                new TypeItem("Mat"),
                new TypeItem("Dumbbell"),
                new TypeItem("Treadmill"),
                new TypeItem("Punching Bag")
            };
            var classes = new List<Class>
            {
                new Class("Cardio Blast", TimeTable.Combine(TimeTable.previousWeekSunday, TimeTable.timeEvening), 25, 120.0m, new List<TypeItem> {
                    typeItems[2], // Dumbbell
                    typeItems[3]  // Treadmill
                }),
                new Class("Yoga Morning", TimeTable.Combine(TimeTable.nextWeekMonday, TimeTable.timeMorning), 40, 100.0m, new List<TypeItem> {
                    typeItems[1]  // Mat
                }),
                new Class("Introduction to Boxing", TimeTable.Combine(TimeTable.nextWeekSunday, TimeTable.timeAfternoon), 30, 150.0m, new List<TypeItem> {
                    typeItems[0], // Bench
                    typeItems[4]  // Punching Bag
                }),
                new Class("Latino Dance", TimeTable.Combine(TimeTable.followingWeekMonday, TimeTable.timeMiddleMorning), 50, 90.0m, new List<TypeItem> {
                    
                })

            };

          //  var controller = new ClassesController(_context, null); // Ignore null for logger in tests
            _context.TypeItems.AddRange(typeItems);
            _context.Classes.AddRange(classes);
            _context.SaveChanges();
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetAvailableClassesForPlan_NULL4NameDate_test()
        {
            // ARRANGE

            List<ClassForPlanDTO> expectedClasses = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(
                    2,
                    "Yoga Morning", 
                    100.0m, 
                    new List<string> { "Mat" },
                    TimeTable.Combine(TimeTable.nextWeekMonday, TimeTable.timeMorning)
                ),
                new ClassForPlanDTO(
                    3, 
                    "Introduction to Boxing", 
                    150.0m, 
                    new List<string> { "Bench", "Punching Bag" },
                    TimeTable.Combine(TimeTable.nextWeekSunday, TimeTable.timeAfternoon)
                )

            };

            var mock = new Mock<ILogger<ClassesController>>();
            ILogger<ClassesController> logger = mock.Object;
            ClassesController controller = new ClassesController(_context, logger); // Can also use null for the logger

            // ACT
            var result = await controller.GetAvailableClassesForPlan(null, null);

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var classesActualResult = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);
            Assert.Equal(expectedClasses, classesActualResult);
        }



        // Transform the fact into a theory
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetAvailableClassesForPlan_OK))]
        public async Task GetAvailableClassesForPlan_theory(string? className, DateOnly? classDate, List<ClassForPlanDTO> expectedClasses)
        {
            // ARRANGE


            var mock = new Mock<ILogger<ClassesController>>();
            ILogger<ClassesController> logger = mock.Object;
            ClassesController controller = new ClassesController(_context, logger); // Can also use null for the logger

            // ACT
            var result = await controller.GetAvailableClassesForPlan(className, classDate);

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var classesActualResult = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);
            Assert.Equal(expectedClasses, classesActualResult);
        }

        public static IEnumerable<object[]> TestCasesFor_GetAvailableClassesForPlan_OK()
        {
            // Here we create the test cases

            // We define the available DTOs here
            var classDTOs = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(
                    2,
                    "Yoga Morning",
                    100.0m,
                    new List<string> { "Mat" },
                    TimeTable.Combine(TimeTable.nextWeekMonday, TimeTable.timeMorning)
                ),
                new ClassForPlanDTO(
                    3,
                    "Introduction to Boxing",
                    150.0m,
                    new List<string> { "Bench", "Punching Bag" },
                    TimeTable.Combine(TimeTable.nextWeekSunday, TimeTable.timeAfternoon)
                ),
            };

            // Then, we prepare the expected results for each test case
            var expectedDTOs_TC1 = new List<ClassForPlanDTO>()
            {
                classDTOs[0], // Yoga Morning
                classDTOs[1], // Introduction to Boxing
            };

            var expectedDTOs_TC2 = new List<ClassForPlanDTO>()
            {
                classDTOs[0], // Yoga Morning
            };

            var expectedDTOs_TC3 = new List<ClassForPlanDTO>() 
            {
                classDTOs[1], // Introduction to Boxing
            };



            // Design all test cases in a collection using the pre-prepared DTOs
            var allTests = new List<object[]> {
                /*new object[] {intput 1, input 2, Prepared DTO for expected result}*/
                new object[] { null, null, expectedDTOs_TC1 },
                new object[] { "Yoga", null, expectedDTOs_TC2 },
                new object[] { null, TimeTable.nextWeekSunday, expectedDTOs_TC3 }
            };

            // Return tests
            return allTests;
        }
    }
}

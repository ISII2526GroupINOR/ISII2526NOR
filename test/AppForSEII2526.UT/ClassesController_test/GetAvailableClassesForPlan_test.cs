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

namespace AppForSEII2526.UT.ClassesController_test
{
    public class GetAvailableClassesForPlan_test : AppForSEII25264SqliteUT
    {
        public static class TimeTable
        {
            // Obtain and calculate some dates for the tests

            public static DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            // These dates might be useful for boundary tests
            public static DateOnly nextWeekMonday = today.DayOfWeek == DayOfWeek.Sunday // Remember: Sunday = 0, Monday = 1, ..., Saturday = 6
                ? today.AddDays(1)
                : today.AddDays(8 - (int)today.DayOfWeek);
            public static DateOnly nextWeekSunday = nextWeekMonday.AddDays(6);
            public static DateOnly followingWeekMonday = nextWeekMonday.AddDays(7);
            public static DateOnly previousWeekSunday = nextWeekMonday.AddDays(-1);

            // These dates might be useful for Equivalence Class tests
            public static DateOnly endsOfTime = today.AddYears(1997); // A date far in the future
            public static DateOnly middleOfNextWeek = nextWeekMonday.AddDays(3); // Middle of next week
            public static DateOnly originOfTime = today.AddYears(-1997); // A date far in the past

            // Some example times
            public static TimeOnly timeMorning = new TimeOnly(9, 0);              // 09:00
            public static TimeOnly timeMiddleMorning = new TimeOnly(11, 30);      // 11:30
            public static TimeOnly timeAfternoon = new TimeOnly(17, 0);           // 17:00
            public static TimeOnly timeEvening = new TimeOnly(19, 0);             // 19:00
            public static TimeOnly timeNight = new TimeOnly(22, 0);               // 22:00
            public static DateTime Combine(DateOnly d, TimeOnly t) => d.ToDateTime(t);

        }


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
                new Class("Cardio Blast", TimeTable.Combine(TimeTable.previousWeekSunday, TimeTable.timeEvening), 25, 120m, new List<TypeItem> {
                    typeItems[2], // Dumbbell
                    typeItems[3]  // Treadmill
                }),
                new Class("Yoga Morning", TimeTable.Combine(TimeTable.nextWeekMonday, TimeTable.timeMorning), 40, 100m, new List<TypeItem> {
                    typeItems[1]  // Mat
                }),
                new Class("Introduction to Boxing", TimeTable.Combine(TimeTable.nextWeekSunday, TimeTable.timeAfternoon), 30, 150m, new List<TypeItem> {
                    typeItems[0], // Bench
                    typeItems[4]  // Punching Bag
                }),
                new Class("Latino Dance", TimeTable.Combine(TimeTable.followingWeekMonday, TimeTable.timeMiddleMorning), 50, 90m, new List<TypeItem> {
                    typeItems[1], // Mat
                })

            };

            var controller = new ClassesController(_context, null); // Ignore null for logger in tests
            _context.TypeItems.AddRange(typeItems);
            _context.Classes.AddRange(classes);
            _context.SaveChanges();
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetAvailableClassesForPlan_NULL4NameDate_test()
        {
            // ARRANGE

            IList<ClassForPlanDTO> expectedClasses = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(
                    2,
                    "Yoga Morning", 
                    100m, 
                    new List<string> { "Mat" },
                    TimeTable.Combine(TimeTable.nextWeekMonday, TimeTable.timeMorning)
                ),
                new ClassForPlanDTO(
                    3, 
                    "Introduction to Boxing", 
                    150m, 
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
    }
}

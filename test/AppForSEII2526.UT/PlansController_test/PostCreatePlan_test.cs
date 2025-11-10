using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.DTOs.ClassDTOs;
using AppForSEII2526.API.DTOs.ApplicationUserDTOs;
using AppForSEII2526.API.Models;
using AppForSEII2526.UT.AuxiliaryClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.DTOs.RestockDTOs;

namespace AppForSEII2526.UT.PlansController_test
{
    public class PostCreatePlan_test : AppForSEII25264SqliteUT
    {
        private static readonly DateTime tnow = TimeTable.now;

        public PostCreatePlan_test()
        {
            // Create model classes for in-memory database context

            // Prepare model classes in the database context (physically stored in main memory for testing)

            // TypeItems needed for Classes
            var typeItems = new List<TypeItem>
            {
                new TypeItem("Bench"),
                new TypeItem("Punching Bag")
            };
            _context.TypeItems.AddRange(typeItems);
            _context.SaveChanges(); // Assign IDs

            // Classes needed for Plans
            var classes = new List<Class>
            {
                new Class("Introduction to Boxing", TimeTable.Combine(TimeTable.nextWeekSunday, TimeTable.timeAfternoon), 30, 150.0m, new List<TypeItem> {
                    typeItems[0], // Bench
                    typeItems[1]  // Punching Bag
                }),
                new Class("Yoga", TimeTable.Combine(TimeTable.middleOfNextWeek, TimeTable.timeMiddleMorning), 1, 100.0m, new List<TypeItem>
                {

                }),
                new Class("Dance", TimeTable.Combine(TimeTable.originOfTime, TimeTable.timeMorning), 10, 300.0m, new List<TypeItem>
                {

                }),
                new Class("Yoga II", TimeTable.Combine(TimeTable.middleOfNextWeek, TimeTable.timeMiddleMorning), 2, 100.0m, new List<TypeItem>
                {

                })
            };
            _context.Classes.AddRange(classes);
            _context.SaveChanges(); // Assign IDs

            // ApplicationUsers needed for PaymentMethods
            var users = new List<ApplicationUser>
            {
                new ApplicationUser("David", "A. Patterson"){Id="1"}
            };
            _context.Users.AddRange(users);
            _context.SaveChanges(); // Assign IDs

            // PaymentMethods needed for Plans
            var paymentMethods = new List<PaymentMethod>
            {
                new PaymentMethod(new List<Plan>(), users[0]),
                new PaymentMethod(new List<Plan>(), null)
            };

            // Plans to be tested
            var plans = new List<Plan>
            {
                new Plan {
                    Name = "Boxing Starter",
                    Description = "This is a starter boxing plan",
                    CreatedDate = tnow,
                    HealthIssues = null,
                    TotalPrice = 150.00m, // Classes: Introduction to Boxing (150.00m)
                    Weeks = 4,
                    PlanItems = new List<PlanItem>{ },
                    PaymentMethod = paymentMethods[0]
                },
                new Plan {
                    Name = "Master Yoga",
                    Description = null,
                    CreatedDate= tnow,
                    HealthIssues = "Back pain",
                    TotalPrice = 100, // Classes: Yoga (100.0m)
                    Weeks=1,
                    PlanItems = new List<PlanItem>{ },
                    PaymentMethod = paymentMethods[1]
                }
            };

            //PaymentMethods need to reference Plans
            paymentMethods[0].Plans!.Add(plans[0]);
            paymentMethods[1].Plans!.Add(plans[1]);

            // Classes need to be added to Plans via PlanItems
            plans[0].PlanItems.Add(new PlanItem
            {
                Plan = plans[0],
                Class = classes[0], // Introduction to Boxing
                Goal = "Learn basic boxing techniques",
                Price = classes[0].Price // 150.00m
            }); // Boxing Starter includes Introduction to Boxing
            plans[1].PlanItems.Add(new PlanItem
            {
                Plan = plans[1],
                Class = classes[1],
                Goal = "Improve",
                Price = classes[1].Price // 100.0m
            });


            // Add remaining objects to context
            _context.PaymentMethods.AddRange(paymentMethods);
            _context.Plans.AddRange(plans);
            _context.SaveChanges();
        }




        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_PostCreatePlan_Error))]
        public async Task PostCreatePlan_Error_test(PlanForCreateDTO planForCreate, string expectedErrorMessage)
        {
            // ARRANGE
            var mock = new Mock<ILogger<PlansController>>();
            ILogger<PlansController> logger = mock.Object;
            PlansController controller = new PlansController(_context, logger);

            // ACT
            var result = await controller.PostCreatePlan(planForCreate);

            // ASSERT
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetail = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var actualErrorMessage = problemDetail.Errors.First().Value[0];

            Assert.StartsWith(actualErrorMessage, expectedErrorMessage);
        }

        [Trait("LevelTesting", "Unit Testing")]
        public static IEnumerable<Object[]> TestCasesFor_PostCreatePlan_Error()
        {
            // Create Input DTOs

            // Bad weeks
            PlanForCreateDTO planBadWeeks = new PlanForCreateDTO
            (
                "newPlan",
                "newDescription",
                null, // health issues
                0, // weeks
                new List<ClassForCreatePlanDTO>(),
                1, // payment method
                "1" // userId
            );

            // Bad PaymentMethod
            PlanForCreateDTO planBadPM = new PlanForCreateDTO
            (
                "newPlan",
                "newDescription",
                null, // health issues
                1, // weeks
                new List<ClassForCreatePlanDTO>(),
                0, // payment method
                "1" // userId
            );

            // Null DataBase User
            PlanForCreateDTO planNullDBUser = new PlanForCreateDTO
            (
                "newPlan",
                "newDescription",
                null, // health issues
                1, // weeks
                new List<ClassForCreatePlanDTO>(),
                2, // payment method
                "2" // userId
            );

            // Mismatch between PaymentMethod and User
            PlanForCreateDTO planMismatchPMU = new PlanForCreateDTO
            (
                "newPlan",
                "newDescription",
                null, // health issues
                1, // weeks
                new List<ClassForCreatePlanDTO>(),
                1, // payment method
                "2" // userId
            );

            // Repeated plan name
            PlanForCreateDTO planRepeatedName = new PlanForCreateDTO
            (
                "Boxing Starter",
                "newDescription",
                null, // health issues
                1, // weeks
                new List<ClassForCreatePlanDTO>(),
                1, // payment method
                "1" // userId
            );

            // Class does not exist
            PlanForCreateDTO planClassNotExist = new PlanForCreateDTO
            (
                "Boxing Starter",
                "newDescription",
                null, // health issues
                1, // weeks
                new List<ClassForCreatePlanDTO>
                {
                    new ClassForCreatePlanDTO(0, null)
                },
                1, // payment method
                "1" // userId
            );

            // Duplicate classes
            PlanForCreateDTO planDuplicateClass = new PlanForCreateDTO
            (
                "Boxing Starter",
                "newDescription",
                null, // health issues
                1, // weeks
                new List<ClassForCreatePlanDTO>
                {
                    new ClassForCreatePlanDTO(1, null),
                    new ClassForCreatePlanDTO(1, null)
                },
                1, // payment method
                "1" // userId
            );

            // Full class
            PlanForCreateDTO planFullClass = new PlanForCreateDTO
            (
                "Master Yoga II",
                "newDescription",
                null, // health issues
                1, // weeks
                new List<ClassForCreatePlanDTO>
                {
                    new ClassForCreatePlanDTO(2 , null),
                },
                1, // payment method
                "1" // userId
            );

            // Class in the past
            PlanForCreateDTO planClassInPast = new PlanForCreateDTO
            (
                "newClass",
                "newDescription",
                null, // health issues
                1, // weeks
                new List<ClassForCreatePlanDTO>
                {
                    new ClassForCreatePlanDTO(3, null),
                },
                1, // payment method
                "1" // userId
            );



            // Set up test cases
            var testCases = new List<Object[]>()
            {
                new object[] { planBadWeeks, "Error! Weeks must be greater than zero." },
                new object[] { planBadPM, $"Error! Payment method with id {planBadPM.PaymentMethodId} does not exist."},
                new object[] { planNullDBUser, $"Error! Payment method with id {planNullDBUser.PaymentMethodId} does not belong to user with id {planNullDBUser.UserId}." },
                new object[] { planMismatchPMU, $"Error! Payment method with id {planMismatchPMU.PaymentMethodId} does not belong to user with id {planMismatchPMU.UserId}." },
                new object[] { planRepeatedName, $"Error! Plan with name {planRepeatedName.Name} already exists" },
                new object[] { planClassNotExist, $"Error! Class with id {planClassNotExist.classes.First().Id} does not exist." },
                new object[] { planDuplicateClass, $"Error! Duplicate class with id {planDuplicateClass.classes.First().Id} found in the plan." },
                new object[] { planFullClass, $"Error! Class with id {planFullClass.classes.First().Id} has reached its capacity limit." },
                new object[] { planClassInPast, $"Error! Class with id {planClassInPast.classes.First().Id} is scheduled in the past." }
            };


            // Return test cases
            return testCases;
        }


        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_PostCreatePlan_Success))]
        public async Task PostCreatePlan_Success_test(PlanForCreateDTO planForCreate, PlanDetailDTO expectedPlanDetailDTO)
        {
            // ARRANGE
            var mock = new Mock<ILogger<PlansController>>();
            ILogger<PlansController> logger = mock.Object;
            PlansController controller = new PlansController(_context, logger);

            // ACT
            var result = await controller.PostCreatePlan(planForCreate);

            // ASSERT
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var actualResultDTO = Assert.IsType<PlanDetailDTO>(created.Value);

            Assert.Equal(expectedPlanDetailDTO, actualResultDTO);
        }


        [Trait("LevelTesting", "Unit Testing")]
        public static IEnumerable<Object[]> TestCasesFor_PostCreatePlan_Success()
        {
            // Create input DTOs

            PlanForCreateDTO planSuccess = new PlanForCreateDTO
            (
                "newPlanSuccess",
                "newDescription",
                null, // health issues
                1, // weeks
                new List<ClassForCreatePlanDTO>
                {
                    new ClassForCreatePlanDTO(1, null),
                    new ClassForCreatePlanDTO (4, "My Goal")
                },
                1, // payment method
                "1" // userId
            );


            // Create expected DTOs
            PlanDetailDTO expectedPlanSuccess = new PlanDetailDTO
            (
                "newPlanSuccess",
                "newDescription",
                TimeTable.now,
                null,
                250.0m, // Classes 1 and 2
                1,
                new ApplicationUserForPlanDetailDTO("1", "David", "A. Patterson"),
                new List<ClassForPlanDTO>
                {
                    new ClassForPlanDTO
                    (
                        1,
                        "Introduction to Boxing",
                        150.0m,
                        new List<string> {"Bench", "Punching Bag"},
                        TimeTable.Combine(TimeTable.nextWeekSunday, TimeTable.timeAfternoon)
                    ),
                    new ClassForPlanDTO
                    (
                        4,
                        "Yoga II",
                        100.0m,
                        new List<string>(),
                        TimeTable.Combine(TimeTable.middleOfNextWeek, TimeTable.timeMiddleMorning)
                    )
                }

            );


            // Set up test cases

            var testCases = new List<Object[]>()
            {
                new object[] {planSuccess, expectedPlanSuccess}
            };

            return testCases;
        }

    }
}

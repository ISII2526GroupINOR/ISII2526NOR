using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.DTOs.ClassDTOs;
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
        private readonly DateTime tnow;

        public PostCreatePlan_test()
        {
            // Create model classes for in-memory database context
            tnow = TimeTable.now;

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
                new PaymentMethod(new List<Plan>(), users[0])
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
                }
            };

            //PaymentMethods need to reference Plans
            paymentMethods[0].Plans!.Add(plans[0]);

            // Classes need to be added to Plans via PlanItems
            plans[0].PlanItems.Add(new PlanItem
            {
                Plan = plans[0],
                Class = classes[0], // Introduction to Boxing
                Goal = "Learn basic boxing techniques",
                Price = classes[0].Price // 150.00m
            }); // Boxing Starter includes Introduction to Boxing


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


            // Set up test cases
            var testCases = new List<Object[]>()
            {
                new object[] { planBadWeeks, "Error! Weeks must be greater than zero." }
            };

            // Return test cases

            return testCases;
        }
    }
}

using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ApplicationUserDTOs;
using AppForSEII2526.API.DTOs.ClassDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.DTOs.RestockDTOs;
using AppForSEII2526.UT.AuxiliaryClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PlansController_test
{
    public class GetPlanDetails_test : AppForSEII25264SqliteUT
    {
        private readonly DateTime tnow = TimeTable.now;
        public GetPlanDetails_test()
        {
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



        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPlanDetails_InvalidID_test()
        {
            // ARRANGE

                // No expected DTO is needed


            // ACT

            var mock = new Mock<ILogger<PlansController>>();
            ILogger<PlansController> logger = mock.Object;
            var controller = new PlansController(_context, logger);

            var result = await controller.GetPlanDetails(0); // ID 0 does not exist


            // ASSERT

            Assert.IsType<NotFoundResult>(result);
        }



        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPlanDetails_ValidID_test()
        {
            // ARRANGE

            // Get plan 1
            var expectedDTO = new PlanDetailDTO(
                "Boxing Starter",
                "This is a starter boxing plan",
                tnow,
                null,
                150.00m,
                4,
                new ApplicationUserForPlanDetailDTO(
                    "1",
                    "David",
                    "A. Patterson"
                ),
                new List<ClassForPlanDTO>()
                {
                    new ClassForPlanDTO(
                        1,
                        "Introduction to Boxing",
                        150.0m,
                        new List<string> {"Bench", "Punching Bag" },
                        TimeTable.Combine(TimeTable.nextWeekSunday, TimeTable.timeAfternoon)
                    )
                }
            );

            // ACT

            var mock = new Mock<ILogger<PlansController>>();
            ILogger<PlansController> logger = mock.Object;
            var controller = new PlansController(_context, logger);

            var result = await controller.GetPlanDetails(1); // ID 1

            // ASSERT

            var okResult = Assert.IsType<OkObjectResult>(result);
            var planDTOactual = Assert.IsType<PlanDetailDTO>(okResult.Value);
            Assert.Equal(expectedDTO, planDTOactual);

        }
    }
}

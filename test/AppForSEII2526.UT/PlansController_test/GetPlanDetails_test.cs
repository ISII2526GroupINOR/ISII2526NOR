using AppForSEII2526.API.Controllers;
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
        public GetPlanDetails_test()
        {
            // Prepare model classes in the database context (physically stored in main memory for testing)

            // TypeItems needed for Classes
            var typeItems = new List<TypeItem>
            {
                new TypeItem("Bench"),
                new TypeItem("Mat"),
                new TypeItem("Punching Bag")
            };

            // Classes needed for Plans
            var classes = new List<Class>
            {
                new Class("Yoga Morning", TimeTable.Combine(TimeTable.nextWeekMonday, TimeTable.timeMorning), 40, 100.0m, new List<TypeItem> {
                    typeItems[1]  // Mat
                }),
                new Class("Introduction to Boxing", TimeTable.Combine(TimeTable.nextWeekSunday, TimeTable.timeAfternoon), 30, 150.0m, new List<TypeItem> {
                    typeItems[0], // Bench
                    typeItems[2]  // Punching Bag
                }),
                new Class("Latino Dance", TimeTable.Combine(TimeTable.followingWeekMonday, TimeTable.timeMiddleMorning), 50, 90.0m, new List<TypeItem> {

                })
            };

            // ApplicationUsers needed for PaymentMethods
            var users = new List<ApplicationUser>
            {
                new ApplicationUser("David", "A. Patterson"),
                new ApplicationUser("John", "LeRoy Hennessy")
            };

            // PaymentMethods needed for Plans
            var paymentMethods = new List<PaymentMethod>
            {
                new PaymentMethod(new List<Plan>(), users[0]),
                new PaymentMethod(new List<Plan>(), users[1])
            };

            // Plans to be tested
            var plans = new List<Plan>
            {
                new Plan {
                    Name = "Boxing Starter",
                    Description = "This is a starter boxing plan",
                    CreatedDate = TimeTable.now,
                    HealthIssues = null,
                    TotalPrice = 150.00m, // Classes: Introduction to Boxing (150.00m)
                    Weeks = 4,
                    PlanItems = new List<PlanItem>{ },
                    PaymentMethod = paymentMethods[0]
                },
                new Plan {
                    Name = "Cardio Plus",
                    Description = "This plan focuses on cardio exercises",
                    CreatedDate = TimeTable.now,
                    HealthIssues = "Back pain",
                    TotalPrice = 190.00m, // Classes: Yoga Morning (100.00m) + Latino Dance (90.00m)
                    Weeks = 6,
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
                Class = classes[1], // Introduction to Boxing
                Goal = "Learn basic boxing techniques",
                Price = classes[1].Price // 150.00m
            }); // Boxing Starter includes Introduction to Boxing

            plans[1].PlanItems.Add(new PlanItem
            {
                Plan = plans[1],
                Class = classes[0], // Yoga Morning
                Goal = "Improve flexibility and mindfulness",
                Price = classes[0].Price // 100.00m
            }); // Cardio Plus includes Yoga Morning

            plans[1].PlanItems.Add(new PlanItem
            {
                Plan = plans[1],
                Class = classes[2], // Latino Dance
                Goal = "Learn Latin dance moves",
                Price = classes[2].Price // 90.00m
            }); // Cardio Plus includes Latino Dance
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPlanDetails_InvalidID_test()
        {
            // ARRANGE

            var mock = new Mock<ILogger<PlansController>>();
            ILogger<PlansController> logger = mock.Object;
            var controller = new PlansController(_context, logger);


            // ACT

            var result = await controller.GetPlanDetails(0); // ID 0 does not exist


            // ASSERT

            Assert.IsType<NotFoundResult>(result);

        }
    }
}

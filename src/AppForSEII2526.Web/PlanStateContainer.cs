using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PlanStateContainer
    {
        // We create an instance of Plan when an instance of PlanStateContainer is created
        public PlanForCreateDTO Plan { get; private set; } = new PlanForCreateDTO() // Has id and goal for each class
        {
            Classes = new List<ClassForCreatePlanDTO>()
        };

        public List<ClassForPlanDTO> ClassesDetail { get; private set; } = new List<ClassForPlanDTO>(); // Has id and all info of each selected class




        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddSelectedClassToPlan(ClassForPlanDTO classForPlan, string goal)
        {
            // Before adding a class, whe check if it has been already added
            if (!Plan.Classes.Any(c => c.Id == classForPlan.Id)) {
                // We add it if it is not in the List
                Plan.Classes.Add(new ClassForCreatePlanDTO()
                {
                    Id = classForPlan.Id,
                    Goal = goal
                });

                ClassesDetail.Add(classForPlan);
            }
        }


        // Delete a class from the list of selected classes
        public void RemoveSelectedClassFromPlan(ClassForPlanDTO classForPlan)
        {
            Plan.Classes.Remove(Plan.Classes.First(c => c.Id == classForPlan.Id));

            // Also remove from the detail classes 
            ClassesDetail.Remove(ClassesDetail.First(c => c.Id == classForPlan.Id));
        }


        // Delete all classes from the list of selected classes
        public void ClearSelectedClassesInPlan()
        {
            Plan.Classes.Clear();
        }


        // When we have finished the process of plan creation, we create a new Plan
        public void PlanCreationProcessed()
        {
            // We have finished the process of creating a plan, so create a new empty object
            Plan = new PlanForCreateDTO()
            {
                Classes = new List<ClassForCreatePlanDTO>()
            };
        }
    }
}

using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PlanStateContainer
    {
        // We create an instance of Plan when an instance of PlanStateContainer is created
        public PlanForCreateDTO Plan { get; private set; } = new PlanForCreateDTO()
        {
            Classes = new List<ClassForCreatePlanDTO>()
        };


        // We compute the TotalPrice of the classes we have selected for inclusion in the plan
        public decimal TotalPrice
        {
            get
            {
                // TODO: Missing price property in ClassForCreatePlanDTO.
                // Price computation is impossible with this approach
                return 0;
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddSelectedClassToPlan(ClassForCreatePlanDTO classForPlan)
        {
            // Before adding a class, whe check if it has been already added
            if (!Plan.Classes.Any(c => c.Id == classForPlan.Id)) {
                // We add it if it is not in the List
                Plan.Classes.Add(new ClassForCreatePlanDTO()
                {
                    Id = classForPlan.Id,
                    Goal = classForPlan.Goal
                });
            }
        }


        // Delete a class from the list of selected classes
        public void RemoveSelectedClassFromPlan(ClassForCreatePlanDTO classForPlan)
        {
            Plan.Classes.Remove(classForPlan);
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

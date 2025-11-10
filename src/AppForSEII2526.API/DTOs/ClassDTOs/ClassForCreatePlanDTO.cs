namespace AppForSEII2526.API.DTOs.ClassDTOs
{
    public class ClassForCreatePlanDTO
    {
        public ClassForCreatePlanDTO()
        {
        }

        public ClassForCreatePlanDTO(int id, string? goal)
        {
            Id = id;
            this.goal = goal;
        }

        public int Id { get; set; } // Id of the selected class

        public string? goal {  get; set; } // Goal of the class
    }
}

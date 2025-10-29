namespace AppForSEII2526.API.DTOs.ApplicationUserDTOs
{
    /// <summary>
    /// DTO for representing ApplicationUser details in the context of a plan.
    /// </summary>
    public class ApplicationUserForPlanDetailDTO
    {
        public ApplicationUserForPlanDetailDTO(string id, string name, string surname)
        {
            Id = id;
            Name = name;
            Surname = surname;
        }

        public string Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Surname { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is ApplicationUserForPlanDetailDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Surname == dTO.Surname;
        }
    }
}

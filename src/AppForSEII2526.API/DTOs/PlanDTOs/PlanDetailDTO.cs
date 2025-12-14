using AppForSEII2526.API.DTOs.ApplicationUserDTOs;
using AppForSEII2526.API.DTOs.ClassDTOs;
using DataType = System.ComponentModel.DataAnnotations.DataType; // To avoid ambiguity
using System; // for Math, TimeSpan (may be provided by global usings)
using System.Linq;

namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class PlanDetailDTO
    {
        public const decimal TimeError = 12.000m; // seconds

        public PlanDetailDTO(string name, string? description, DateTime createdDate, string? healthIssues, decimal totalPrice, int weeks, ApplicationUserForPlanDetailDTO? user, IList<ClassForPlanDTO> classes)
        {
            Name = name;
            Description = description;
            CreatedDate = createdDate;
            HealthIssues = healthIssues;
            TotalPrice = totalPrice;
            Weeks = weeks;
            User = user;
            Classes = classes;
        }

        // Created along with the Id property to allow the Web client to identify the plan and call the API method GetPlanDetail
        public PlanDetailDTO(int id, string name, string? description, DateTime createdDate, string? healthIssues, decimal totalPrice, int weeks, ApplicationUserForPlanDetailDTO? user, IList<ClassForPlanDTO> classes)
            : this(name, description, createdDate, healthIssues, totalPrice, weeks, user, classes)
        {
            Id = id;
        }

        public int Id { get; set; } // Used to allow the Web client to identify the plan and call the API method GetPlanDetail

        [StringLength(128, ErrorMessage = "Plan name cannot be longer than 128 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Plan name can only contain letters, numbers, spaces, and hyphens.")]
        public string Name { get; set; }

        [StringLength(1024, ErrorMessage = "Plan description cannot be longer than 1024 characters.")]
        public string? Description { get; set; }

        [DataType(DataType.DateTime)] // Used DataType type to take into account both date and time, as defined in the requirements.
        public DateTime CreatedDate { get; set; } // Not null

        [StringLength(4096, ErrorMessage = "Health issues cannot be longer than 4096 characters.")]
        [Display(Name = "Health Issues")]
        public string? HealthIssues { get; set; }

        [Precision(5, 2)]
        [DataType(DataType.Currency)]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; } // Not null

        [Range(1, 52, ErrorMessage = "Weeks must be between 1 and 52 (1 year).")]
        public int Weeks { get; set; } // Not null

        public ApplicationUserForPlanDetailDTO? User { get; set; }

        public IList<ClassForPlanDTO> Classes { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PlanDetailDTO dTO &&
                   Name == dTO.Name &&
                   Description == dTO.Description &&
                   // Compare CreatedDate as UTC with an absolute tolerance of TimeError seconds
                   Math.Abs((CreatedDate.ToUniversalTime() - dTO.CreatedDate.ToUniversalTime()).TotalSeconds) <= (double)TimeError &&
                   HealthIssues == dTO.HealthIssues &&
                   TotalPrice == dTO.TotalPrice &&
                   Weeks == dTO.Weeks &&
                   EqualityComparer<ApplicationUserForPlanDetailDTO?>.Default.Equals(User, dTO.User) &&
                   Classes.SequenceEqual(dTO.Classes);
        }
    }
}

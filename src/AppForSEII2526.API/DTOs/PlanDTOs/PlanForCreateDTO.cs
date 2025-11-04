using AppForSEII2526.API.DTOs.ClassDTOs;
using DataType = System.ComponentModel.DataAnnotations.DataType; // To avoid ambiguity


namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class PlanForCreateDTO
    {
        [Required]
        [StringLength(128, ErrorMessage = "Plan name cannot be longer than 128 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Plan name can only contain letters, numbers, spaces, and hyphens.")]
        public string Name { get; set; } 

        [StringLength(1024, ErrorMessage = "Plan description cannot be longer than 1024 characters.")]
        public string? Description { get; set; }

        [StringLength(4096, ErrorMessage = "Health issues cannot be longer than 4096 characters.")]
        [Display(Name = "Health Issues")]
        public string? HealthIssues { get; set; }

        [Required]
        [Range(1, 52, ErrorMessage = "Weeks must be between 1 and 52 (1 year).")]
        public int Weeks { get; set; } // Mandatory

        [Required] // This frees the controller from null-checking
        public IList<ClassForCreatePlanDTO> classes { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

        // TODO: Missing PaymentMethod information
    }
}

using DataType = System.ComponentModel.DataAnnotations.DataType; // To avoid ambiguity

namespace AppForSEII2526.API.Models
{
    [Index (nameof(Name), IsUnique = true)]

    public class Plan
    {
        public Plan()
        {
            // Ignore warning about uninitialized non-nullable properties.
        }

        public Plan(string name, string? description, DateTime createdDate, string? healthIssues, decimal totalPrice, int weeks, IList<PlanItem> planItems, PaymentMethod? paymentMethod)
        {
            Name = name;
            Description = description;
            CreatedDate = createdDate;
            HealthIssues = healthIssues;
            TotalPrice = totalPrice;
            Weeks = weeks;
            PlanItems = planItems;
            PaymentMethod = paymentMethod;
        }


        public Plan(string name, string? description, DateTime createdDate, string? healthIssues, decimal totalPrice, int weeks, PaymentMethod? paymentMethod)
        {
            // Ignore warning about uninitialized non-nullable properties.
            Name = name;
            Description = description;
            CreatedDate = createdDate;
            HealthIssues = healthIssues;
            TotalPrice = totalPrice;
            Weeks = weeks;
            PaymentMethod = paymentMethod;
        }



        // Entity attributes

        public int Id { get; set; } // Primary key

        [Required]
        [StringLength(128, ErrorMessage = "Plan name cannot be longer than 128 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Plan name can only contain letters, numbers, spaces, and hyphens.")]
        public required string Name { get; set; } // Not null, unique

        [StringLength(1024, ErrorMessage = "Plan description cannot be longer than 1024 characters.")]
        public string? Description { get; set; }

        [DataType(DataType.DateTime)] // Used DataType type to take into account both date and time, as defined in the requirements.
        public DateTime CreatedDate { get; set; } // Not null

        [StringLength(4096, ErrorMessage = "Health issues cannot be longer than 4096 characters.")]
        [Display(Name = "Health Issues")]
        public string? HealthIssues { get; set; }

        [Precision(5,2)]
        [DataType(DataType.Currency)]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; } // Not null

        [Range(1, 52, ErrorMessage = "Weeks must be between 1 and 52 (1 year).")]
        public int Weeks { get; set; } // Not null


        // RELATIONSHIPS
        public IList<PlanItem> PlanItems { get; set; } 

        public PaymentMethod? PaymentMethod { get; set; } // Optional relationship

    }
}

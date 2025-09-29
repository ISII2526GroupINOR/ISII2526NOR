using DataType = System.ComponentModel.DataAnnotations.DataType; // To avoid ambiguity

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(PlanId), nameof(ClassId))]
    public class PlanItem
    {
        // Foreign keys
        public Plan Plan { get; set; }
        public int PlanId { get; set; }

        public Class Class { get; set; }
        public int ClassId { get; set; }

        // Entity attributes

        [Required]
        [StringLength(4096, ErrorMessage = "Goal information cannot contain more than 4096 characters.")]
        public required string Goal { get; set; }

        [Precision(5, 2)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; } 
    }
}

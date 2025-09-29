using DataType = System.ComponentModel.DataAnnotations.DataType; // To avoid ambiguity

namespace AppForSEII2526.API.Models
{
    public class PlanItem
    {
        public int Id { get; set; } // Primary key

        [Required]
        public required string Goal { get; set; }

        
        public int PlanId { get; set; }

        [Precision(5, 2)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }



        // Foreign key
        public Plan Plan { get; set; }
    }
}

namespace AppForSEII2526.API.Models
{
    [Index (nameof(Id), IsUnique = true)]
    public class Purchase
    {

        public int Id { get; set; }
        [RegularExpression(@"^[A-Z][a-zA-z ]*$")]
        [Required]
        public required string City { get; set; }
        [RegularExpression(@"^[A-Z][a-zA-z ]*$")]
        [Required]
        public required string Country { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        [RegularExpression(@"^[A-Z][a-zA-z ]*$")]
        [Required]
        public required string Street { get; set; }
        [Precision(5,2)]
        public decimal TotalPrice { get; set; }

        public IList<PurchaseItem>? PurchaseItems { get; set; }

        public PaymentMethod? paymentMethod { get; set; }



    }
}

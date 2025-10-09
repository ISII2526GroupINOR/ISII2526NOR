namespace AppForSEII2526.API.Models
{
    public class CreditCard: PaymentMethod
    {
        [Required]
        public required string CreditCardNumber { get; set; }
        [Required]
        public required DateTime ExpirationDate { get; set; }

    }
}

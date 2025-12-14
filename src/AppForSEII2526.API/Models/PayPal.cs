namespace AppForSEII2526.API.Models
{
    public class PayPal: PaymentMethod
    {
        [Required]
        public required string Email { get; set; }

        public override string ToString()
        {
            return $"Email: {Email}";
        }
    }
}

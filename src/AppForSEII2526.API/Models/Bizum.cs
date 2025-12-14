namespace AppForSEII2526.API.Models
{
    public class Bizum : PaymentMethod
    {
        [Required]
        public required long TelephoneNumber { get; set; }

        public override string ToString()
        {
            return $"Telephone: {TelephoneNumber}";
        }
    }


}

namespace AppForSEII2526.API.Models
{
    [Index (nameof(Id), IsUnique = true)]
    public class Purchase
    {
        public Purchase(int id, string city, string country, string street, 
            DateTime date, string? description, decimal totalPrice, 
            IList<PurchaseItem>? purchaseItems, PaymentMethod paymentMethod)
        {
            Id = id;
            City = city;
            Country = country;
            Date = date;
            Description = description;
            Street = street;
            TotalPrice = totalPrice;
            PurchaseItems = purchaseItems;
            this.paymentMethod = paymentMethod;
        }

        public int Id { get; set; }
        [RegularExpression(@"^[A-Z][a-zA-z ]*$")]
        [Required]
        public string City { get; set; }
        [RegularExpression(@"^[A-Z][a-zA-z ]*$")]
        [Required]
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        [RegularExpression(@"^[A-Z][a-zA-z ]*$")]
        [Required]
        public string Street { get; set; }
        [Precision(5,2)]
        public decimal TotalPrice { get; set; }

        public IList<PurchaseItem>? PurchaseItems { get; set; }

        public PaymentMethod? paymentMethod { get; set; }



    }
}

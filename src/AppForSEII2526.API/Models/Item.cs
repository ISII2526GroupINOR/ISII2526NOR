namespace AppForSEII2526.API.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        [StringLength(128, ErrorMessage = "Item name cannot be longer than 128 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Item name can only contain letters, numbers, spaces, and hyphens.")]
        public string Name { get; set; }
        [StringLength(1024, ErrorMessage = "Item description cannot be longer than 1024 characters.")]
        public string Description { get; set; }
        [Required]
        [Precision(5,2)]
        public decimal PurchasePrice { get; set; }
        [Required]
        public int QuantityAvailableForPurchase { get; set; }
        [Required]
        public int QuantityForRestock { get; set; }
        [Required]
        [Precision(5,2)]
        public decimal? RestockPrice { get; set; }
    }
}

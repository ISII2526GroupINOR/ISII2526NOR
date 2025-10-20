namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForRestockingDTO {
        public ItemForRestockingDTO(int id, string name, string brandName)
        {
            Id = id;
            Name = name;
            Brand = brandName;
        }

        public ItemForRestockingDTO(int id, string name, string brand, decimal? restockPrice, int quantityForRestock) : this(id, name, brand)
        {
            RestockPrice = restockPrice;
            QuantityForRestock = quantityForRestock;
        }

        public ItemForRestockingDTO(int id, string name, string brand, decimal? restockPrice, int quantityForRestock, int quantityAvailableForPurchase) : this(id, name, brand, restockPrice, quantityForRestock)
        {
            QuantityAvailableForPurchase = quantityAvailableForPurchase;
        }

        public int Id { get; set; }
        [Required]
        [StringLength(128, ErrorMessage = "Item name cannot be longer than 128 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Item name can only contain letters, numbers, spaces, and hyphens.")]
        public string Name { get; set; }
        public string Brand { get; set; }
        [Required]
        [Precision(5, 2)]
        public decimal? RestockPrice { get; set; }
        [Required]
        public int QuantityForRestock { get; set; }
        [Required]
        public int QuantityAvailableForPurchase { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemForRestockingDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   RestockPrice == dTO.RestockPrice &&
                   QuantityForRestock == dTO.QuantityForRestock &&
                   QuantityAvailableForPurchase == dTO.QuantityAvailableForPurchase;
        }
    }
}


namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForCreateRestockDTO
    {
        public ItemForCreateRestockDTO(int id, string name, string brand, decimal? restockPrice, int quantityForRestock, int quantityAvailableForPurchase, int restockQuantity, int item)
        {
            Id = id;
            Name = name;
            Brand = brand;
            RestockPrice = restockPrice;
            QuantityForRestock = quantityForRestock;
            QuantityAvailableForPurchase = quantityAvailableForPurchase;
            RestockQuantity = restockQuantity;
            Item = item;
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
        public int RestockQuantity { get; set; } //Quantity that the admin has specified to be restocked
        public int Item { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemForCreateRestockDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   RestockPrice == dTO.RestockPrice &&
                   QuantityForRestock == dTO.QuantityForRestock &&
                   QuantityAvailableForPurchase == dTO.QuantityAvailableForPurchase &&
                   RestockQuantity == dTO.RestockQuantity &&
                   Item == dTO.Item;
        }
    }
}

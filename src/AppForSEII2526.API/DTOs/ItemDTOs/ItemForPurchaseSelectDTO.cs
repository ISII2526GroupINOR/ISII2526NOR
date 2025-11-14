
namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForPurchaseSelectDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(128, ErrorMessage = "Item name cannot be longer than 128 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Item name can only contain letters, numbers, spaces, and hyphens.")]
        public string Name { get; set; }

        public ItemForPurchaseSelectDTO(int id, string name, string brand, string description, int quantityAvailableForPurchase, decimal purchasePrice)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Description = description;
            QuantityAvailableForPurchase = quantityAvailableForPurchase;
            PurchasePrice = purchasePrice;
        }


        public string Brand { get; set; }
        public string Description { get; set; }

        [Required]
        public int QuantityAvailableForPurchase { get; set; }

        [Required]
        [Precision(5, 2)]
        public decimal PurchasePrice { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemForPurchaseSelectDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   Description == dTO.Description &&
                   QuantityAvailableForPurchase == dTO.QuantityAvailableForPurchase &&
                   PurchasePrice == dTO.PurchasePrice;
        }
    }
}

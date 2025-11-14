
namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForPurchaseCreateDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(128, ErrorMessage = "Item name cannot be longer than 128 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Item name can only contain letters, numbers, spaces, and hyphens.")]
        public string Name { get; set; }

        public ItemForPurchaseCreateDTO(int id, string name, string brand, string description, int quantity, decimal purchasePrice)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Description = description;
            Quantity = quantity;
            PurchasePrice = purchasePrice;
        }



        public string Brand { get; set; }
        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Precision(5, 2)]
        public decimal PurchasePrice { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemForPurchaseCreateDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   Description == dTO.Description &&
                   Quantity == dTO.Quantity &&
                   PurchasePrice == dTO.PurchasePrice;
        }
    }
}

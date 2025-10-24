namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForPurchaseDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(128, ErrorMessage = "Item name cannot be longer than 128 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Item name can only contain letters, numbers, spaces, and hyphens.")]
        public string Name { get; set; }

        public ItemForPurchaseDTO(int id, string name, TypeItem typeItem, Brand brand, string description, int quantityAvailableForPurchase, decimal purchasePrice)
        {
            Id = id;
            Name = name;
            //Brand = brand;
            //Description = description;
            //QuantityAvailableForPurchase = quantityAvailableForPurchase;
            //PurchasePrice = purchasePrice;
            TypeItem = typeItem;
            Brand = brand;
            Description = description;
            QuantityAvailableForPurchase = quantityAvailableForPurchase;
            PurchasePrice = purchasePrice;
        }

        public TypeItem TypeItem { get; set; }

        public Brand Brand { get; set; }
        public string Description { get; set; }

        [Required]
        public int QuantityAvailableForPurchase { get; set; }
        [Required]
        [Precision(5, 2)]
        public decimal PurchasePrice { get; set; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
    }
}
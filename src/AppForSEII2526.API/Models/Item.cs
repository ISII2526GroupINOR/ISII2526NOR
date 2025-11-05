namespace AppForSEII2526.API.Models
{
    public class Item
    {
        public Item()
        {
        }

        public Item(string name, string description, decimal purchasePrice, int quantityAvailableForPurchase, int quantityForRestock, decimal? restockPrice, Brand brand, TypeItem typeItem)
        {
            Name = name;
            Description = description;
            PurchasePrice = purchasePrice;
            QuantityAvailableForPurchase = quantityAvailableForPurchase;
            QuantityForRestock = quantityForRestock;
            RestockPrice = restockPrice;
            Brand = brand;
            TypeItem = typeItem;
        }

        public Item(int id, string name, IList<PurchaseItem> purchaseItems, string description, decimal purchasePrice, int quantityAvailableForPurchase, int quantityForRestock, decimal? restockPrice, IList<RestockItem> restockItems, Brand brand, TypeItem typeItem)
        {
            Id = id;
            Name = name;
            PurchaseItems = purchaseItems;
            Description = description;
            PurchasePrice = purchasePrice;
            QuantityAvailableForPurchase = quantityAvailableForPurchase;
            QuantityForRestock = quantityForRestock;
            RestockPrice = restockPrice;
            RestockItems = restockItems;
            Brand = brand;
            TypeItem = typeItem;
        }

        public int Id { get; set; }
        [Required]
        [StringLength(128, ErrorMessage = "Item name cannot be longer than 128 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Item name can only contain letters, numbers, spaces, and hyphens.")]
        public string Name { get; set; }
        [StringLength(1024, ErrorMessage = "Item description cannot be longer than 1024 characters.")]
        public IList<PurchaseItem> PurchaseItems { get; set; }
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
        public IList<RestockItem> RestockItems { get; set; }
        public Brand Brand { get; set; }
        public TypeItem TypeItem { get; set; }
    }
}

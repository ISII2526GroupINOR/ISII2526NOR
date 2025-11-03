namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ItemId), nameof(RestockId))]
    public class RestockItem
    {
        public RestockItem()
        {
        }

        public RestockItem(int quantity, Restock restock, Item item)
        {
            Quantity = quantity;
            Restock = restock;
            RestockId = restock.Id;
            Item = item;
            ItemId = Item.Id;
        }

        public RestockItem(int quantity, decimal? restockPrice, Restock restock, Item item)
        {
            Quantity = quantity;
            RestockPrice = restockPrice;
            Restock = restock;
            RestockId = restock.Id;
            Item = item;
            ItemId = Item.Id;
        }

        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int RestockId { get; set; }
        [Precision(5,2)]
        public decimal? RestockPrice { get; set; }
        public Restock Restock { get; set; }
        public Item Item { get; set; }
    }
}

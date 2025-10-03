namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ItemId), nameof(PurchaseId))]
    public class PurchaseItem
    {
        public Item? Item { get; set; }
        public int ItemId { get; set; }

        public Purchase? Purchase {  get; set; }
        public int PurchaseId { get; set; }

        public int AmountBought { get; set; }

        [Precision(5, 2)]
        public decimal Price { get; set; }


    }
}

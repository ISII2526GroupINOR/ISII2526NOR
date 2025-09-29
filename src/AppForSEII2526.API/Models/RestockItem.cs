namespace AppForSEII2526.API.Models
{
    public class RestockItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int RestockId { get; set; }
        [Precision(5,2)]
        public decimal? RestockPrice { get; set; }
        public Restock Restock { get; set; }
    }
}

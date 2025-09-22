namespace AppForSEII2526.API.Models
{
    [Index(nameof(Title), IsUnique = true)]
    public class Restock
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DeliveryAddress {  get; set; }
        public string Description { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime RestockDate { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}

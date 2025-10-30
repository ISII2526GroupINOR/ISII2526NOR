namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseDetailDTO
    {
        public PurchaseDetailDTO(int id, string? description, string delivery_address, decimal total_price, string payment_method, List<PurchaseItem> items)
        {
            Id = id;
            Description = description;
            Delivery_address = delivery_address;
            Total_price = total_price;
            Payment_method = payment_method;
            List<PurchaseItem> Items = items;
        }

        public int Id { get; set; }
        public string? Description { get; set; } = null;
        public string Delivery_address { get; set; }
        public decimal Total_price { get; set; }
        public string Payment_method { get; set; }
        public List<PurchaseItem> Items { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseDetailDTO dTO &&
                   Id == dTO.Id &&
                   Description == dTO.Description &&
                   Delivery_address == dTO.Delivery_address &&
                   Total_price == dTO.Total_price &&
                   Payment_method == dTO.Payment_method &&
                   EqualityComparer<List<PurchaseItem>>.Default.Equals(Items, dTO.Items);
        }
    }

}

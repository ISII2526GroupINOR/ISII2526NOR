namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseDetailDTO
    {
        public PurchaseDetailDTO(int id, string? description, string street, string city, 
            string country, decimal total_price, string payment_method, List<PurchaseItem> items)
        {
            Id = id;
            Description = description;
            Street = street;
            City = city;
            Country = country;
            Total_price = total_price;
            Payment_method = payment_method;
            List<PurchaseItem> Items = items;
        }

        public int Id { get; set; }
        public string? Description { get; set; } = null;

        public string Street { get; set; } 
        public string City { get; set; }
        public string Country { get; set; }
        public string Delivery_address { get; set; }
        public decimal Total_price { get; set; }
        public string Payment_method { get; set; }
        public List<PurchaseItem> Items { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseDetailDTO dTO &&
                   Id == dTO.Id &&
                   Description == dTO.Description &&
                   Street == dTO.Street &&
                   City == dTO.City &&
                   Country == dTO.Country &&
                   Delivery_address == dTO.Delivery_address &&
                   Total_price == dTO.Total_price &&
                   Payment_method == dTO.Payment_method &&
                   EqualityComparer<List<PurchaseItem>>.Default.Equals(Items, dTO.Items);
        }
    }

}

using AppForSEII2526.API.DTOs.ItemDTOs;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseDetailDTO
    {
        public PurchaseDetailDTO(int id, string? description, string street, string city, 
            string country, decimal total_price, PaymentMethod payment_method, IList<ItemForPurchaseCreateDTO> items)
        {
            Id = id;
            Description = description;
            Street = street;
            City = city;
            Country = country;
            Total_price = total_price;
            Payment_method = payment_method;
            IList<ItemForPurchaseCreateDTO> Items = items;
        }

        public int Id { get; set; }
        public string? Description { get; set; } = null;

        public string Street { get; set; } 
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Total_price { get; set; }
        public PaymentMethod Payment_method { get; set; }
        public IList<ItemForPurchaseCreateDTO> Items { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseDetailDTO dTO &&
                   Id == dTO.Id &&
                   Description == dTO.Description &&
                   Street == dTO.Street &&
                   City == dTO.City &&
                   Country == dTO.Country &&
                   Total_price == dTO.Total_price &&
                   EqualityComparer<PaymentMethod>.Default.Equals(Payment_method, dTO.Payment_method) &&
                   EqualityComparer<IList<ItemForPurchaseCreateDTO>>.Default.Equals(Items, dTO.Items);
        }
    }

}

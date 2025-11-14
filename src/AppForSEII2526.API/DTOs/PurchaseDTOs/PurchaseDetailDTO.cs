using AppForSEII2526.API.DTOs.ItemDTOs;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseDetailDTO
    {

        public PurchaseDetailDTO(int id, string? description, string street, string city,
            string country, decimal total_price, int paymentMethodId, IList<ItemForPurchaseCreateDTO>? items)
        {
            Id = id;
            Description = description;
            Street = street;
            City = city;
            Country = country;
            Total_price = total_price;
            PaymentMethodId = paymentMethodId;
            IList<ItemForPurchaseCreateDTO> Items = items;
        }



        public int Id { get; set; }
        public string? Description { get; set; } = null;

        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Total_price { get; set; }
        public int PaymentMethodId { get; set; }
        public IList<ItemForPurchaseCreateDTO> Items { get; set; }



    }
}

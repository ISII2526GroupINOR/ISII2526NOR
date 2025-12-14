

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PaymentMethodForPurchaseDTO
    {
        public PaymentMethodForPurchaseDTO(int id, string type, string description)
        {
            Id = id;
            Type = type;
            Description = description;
        }



        public int Id { get; set; }
        
        public string Type { get; set; }

        public string Description { get; set; }



    }
}

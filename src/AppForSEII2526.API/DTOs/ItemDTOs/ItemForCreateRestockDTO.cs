
namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForCreateRestockDTO
    {
        public ItemForCreateRestockDTO(int id, int restockQuantity)
        {
            Id = id;
            RestockQuantity = restockQuantity;
        }

        public int Id { get; set; }
        public int RestockQuantity { get; set; } //Quantity that the admin has specified to be restocked

        public override bool Equals(object? obj)
        {
            return obj is ItemForCreateRestockDTO dTO &&
                   Id == dTO.Id &&
                   RestockQuantity == dTO.RestockQuantity;
        }
    }
}

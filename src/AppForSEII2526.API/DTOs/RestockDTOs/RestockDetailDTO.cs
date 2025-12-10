using AppForSEII2526.API.DTOs.ItemDTOs;

namespace AppForSEII2526.API.DTOs.RestockDTOs
{
    public class RestockDetailDTO
    {
        public RestockDetailDTO(int id, string title, string deliveryAddress, string? description, DateTime expectedDate, decimal? totalPrice, IList<ItemForRestockingDTO> restockItems, string name, string surname)
        {
            Id = id;
            Title = title;
            DeliveryAddress = deliveryAddress;
            Description = description;
            ExpectedDate = expectedDate;
            TotalPrice = totalPrice;
            RestockItems = restockItems;
            Name = name;
            Surname = surname;
        }

        public int Id { get; set; }
        [StringLength(50, ErrorMessage = "Title can´t be more than 50 characters")]
        public string Title { get; set; }
        public string DeliveryAddress { get; set; }
        [StringLength(100, ErrorMessage = "Description can´t be more than 100 characters.")]
        public string? Description { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime ExpectedDate { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Precision(5, 2)]
        public decimal? TotalPrice { get; set; }
        public IList<ItemForRestockingDTO> RestockItems { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RestockDetailDTO dTO &&
                   Id == dTO.Id &&
                   Title == dTO.Title &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   Description == dTO.Description &&
                   ExpectedDate == dTO.ExpectedDate &&
                   TotalPrice == dTO.TotalPrice &&
                   RestockItems.SequenceEqual(dTO.RestockItems) &&
                   Name == dTO.Name &&
                   Surname == dTO.Surname;
        }
    }
}

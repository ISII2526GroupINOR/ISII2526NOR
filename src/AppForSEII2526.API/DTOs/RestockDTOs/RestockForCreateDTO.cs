
using AppForSEII2526.API.DTOs.ItemDTOs;

namespace AppForSEII2526.API.DTOs.RestockDTOs
{
    public class RestockForCreateDTO
    {
        public RestockForCreateDTO()
        {
        }

        public RestockForCreateDTO(string title, string deliveryAddress, string? description, DateTime expectedDate, DateTime restockDate, decimal? totalPrice, IList<RestockItem> restockItems, ApplicationUser restockResponsible)
        {
            Title = title;
            DeliveryAddress = deliveryAddress;
            Description = description;
            ExpectedDate = expectedDate;
            RestockDate = restockDate;
            TotalPrice = totalPrice;
            RestockItems = restockItems;
            RestockResponsible = restockResponsible;
        }

        public string Title { get; set; }
        public string DeliveryAddress { get; set; }
        [StringLength(100, ErrorMessage = "Description can´t be more than 100 characters.")]
        public string? Description { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime ExpectedDate { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime RestockDate { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Precision(5, 2)]
        public decimal? TotalPrice { get; set; }
        public IList<RestockItem> RestockItems { get; set; }
        public ApplicationUser RestockResponsible { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RestockForCreateDTO dTO &&
                   Title == dTO.Title &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   Description == dTO.Description &&
                   ExpectedDate == dTO.ExpectedDate &&
                   RestockDate == dTO.RestockDate &&
                   TotalPrice == dTO.TotalPrice &&
                   EqualityComparer<IList<RestockItem>>.Default.Equals(RestockItems, dTO.RestockItems) &&
                   EqualityComparer<ApplicationUser>.Default.Equals(RestockResponsible, dTO.RestockResponsible);
        }
    }
}

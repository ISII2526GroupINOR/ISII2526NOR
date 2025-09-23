namespace AppForSEII2526.API.Models
{
    [Index(nameof(Title), IsUnique = true)]
    public class Restock
    {
        public int Id { get; set; }
        [StringLength(50, ErrorMessage ="Title can´t be more than 50 characters")]
        public string Title { get; set; }
        public string DeliveryAddress {  get; set; }
        [StringLength(100, ErrorMessage ="Description can´t be more than 100 characters.")]
        public string? Description { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime ExpectedDate { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime RestockDate { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Precision(5,2)]
        public decimal TotalPrice { get; set; }
    }
}

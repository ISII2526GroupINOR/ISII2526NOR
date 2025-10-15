namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForRestockingDTO {
        public ItemForRestockingDTO(int id, string name, string brandName)
        {
            Id = id;
            Name = name;
            Brand = brandName;
        }

        public int Id { get; set; }
        [Required]
        [StringLength(128, ErrorMessage = "Item name cannot be longer than 128 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Item name can only contain letters, numbers, spaces, and hyphens.")]
        public string Name { get; set; }
        public string Brand { get; set; }
    }
}

namespace AppForSEII2526.API.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        [StringLength(128, ErrorMessage = "Brand name cannot be longer than 128 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Brand name can only contain letters, numbers, spaces, and hyphens.")]
        public string Name { get; set; }
        public IList<Item> Items { get; set; }
    }
}

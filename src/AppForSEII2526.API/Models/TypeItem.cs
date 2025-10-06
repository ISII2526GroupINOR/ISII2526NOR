namespace AppForSEII2526.API.Models
{
    public class TypeItem
    {
        public int Id { get; set; } // Primary key

        [Required]
        [StringLength(128, ErrorMessage = "TypeItem name cannot be longer than 128 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "TypeItem name can only contain letters, numbers, spaces, and hyphens.")]
        public required string Name { get; set; }

        public IList<Item> Items { get; set; }
    }
}

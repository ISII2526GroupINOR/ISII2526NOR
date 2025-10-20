
namespace AppForSEII2526.API.DTOs.ClassDTOs
{
    public class ClassForPlanDTO
    {
        public ClassForPlanDTO(int id, string name, IList<TypeItem> types)
        {
            Id = id;
            Name = name;
            TypeItems = types;
        }

        public int Id { get; set; }

        
        [StringLength(256, ErrorMessage = "Class name cannot have more than 256 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Class name can only contain alphanumeric characters and spaces.")]
        public string Name { get; set; }

        public IList<TypeItem> TypeItems { get; set; }
    }
}

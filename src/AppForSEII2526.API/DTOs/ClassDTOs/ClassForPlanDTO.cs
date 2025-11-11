using DataType = System.ComponentModel.DataAnnotations.DataType; 


namespace AppForSEII2526.API.DTOs.ClassDTOs
{
    public class ClassForPlanDTO
    {
        public ClassForPlanDTO(int id, string name, decimal price, IList<string> typeItems, DateTime date)
        {
            Id = id;
            Name = name;
            Price = price;
            TypeItems = typeItems;
            Date = date;
        }

        public int Id { get; set; }

        
        [StringLength(256, ErrorMessage = "Class name cannot have more than 256 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Class name can only contain alphanumeric characters and spaces.")]
        public string Name { get; set; }

        [Precision(5, 2)]
        public decimal Price { get; set; }

        public IList<string> TypeItems { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ClassForPlanDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Price == dTO.Price &&
                   TypeItems.SequenceEqual(dTO.TypeItems);
        }
    }
}

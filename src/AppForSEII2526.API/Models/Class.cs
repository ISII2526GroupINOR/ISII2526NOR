using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)] // Search index on Name, must be unique

    public class Class
    {
        public Class()
        {
            // Ignore warning about uninitialized non-nullable properties.
        }

        public Class(string name, DateTime date, int capacity, decimal price)
        {
            // Ignore warning about uninitialized non-nullable properties.
            Name = name;
            Date = date;
            Capacity = capacity;
            Price = price;
        }

        // Created for GetAvailableClassesForPlan_test
        public Class(string name, DateTime date, int capacity, decimal price, IList<TypeItem> typeItems) : this(name, date, capacity, price)
        {
            TypeItems = typeItems;
        }


        public int Id { get; set; } // Primary key

        [Required]
        [StringLength(256, ErrorMessage = "Class name cannot have more than 256 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Class name can only contain alphanumeric characters and spaces.")]
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public int Capacity { get; set; }

        [Precision(5, 2)]
        public decimal Price { get; set; }

        // Navigation properties - Relationships
        public IList<PlanItem> PlanItems { get; set; }

        public IList<TypeItem> TypeItems { get; set; }
    }
}

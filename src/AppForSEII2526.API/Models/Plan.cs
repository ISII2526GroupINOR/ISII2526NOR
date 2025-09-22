namespace AppForSEII2526.API.Models
{
    [Index (nameof(Name), IsUnique = true)]
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string HealthIssues { get; set; }
        [Precision(5,2)]
        public decimal TotalPrice { get; set; }
        public int Weeks { get; set; }


    }
}

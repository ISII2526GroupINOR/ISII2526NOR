using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    // Id is a string and is inherited from IdentityUser

    [MaxLength(128)]
    public string? Name { get; set; }

    [MaxLength(256)]
    public string? Surname { get; set; }

    public IList<Plan>? Plans { get; set; }


    // Navigation properties for related entities
    public IList<PaymentMethod>? PaymentMethods { get; set; }
    public IList<Restock> Restocks { get; set; }
}
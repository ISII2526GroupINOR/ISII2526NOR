using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    public ApplicationUser()
    {
        // Ignore warning about uninitialized non-nullable properties.
    }

    public ApplicationUser(string name, string surname) // Lists not included
    {
        Name = name;
        Surname = surname;
    }

    public ApplicationUser(string name, string surname, string username) // Lists not included
    {
        Name = name;
        Surname = surname;
        UserName = username;
    }

    // Id is a string and is inherited from IdentityUser

    //[Required] // This attribute ensures the database column is non-nullable
    [MaxLength(128)]
    public string? Name { get; set; } // There is no compile-time check for nullability, so we avoid constructors (compile errors) for simplicity

    //[Required] // This attribute ensures the database column is non-nullable
    [MaxLength(256)]
    public string? Surname { get; set; } // There is no compile-time check for nullability, so we avoid constructors (compile errors) for simplicity

    public IList<Plan>? Plans { get; set; }


    // Navigation properties for related entities
    public IList<PaymentMethod>? PaymentMethods { get; set; }
    public IList<Restock> Restocks { get; set; }
}
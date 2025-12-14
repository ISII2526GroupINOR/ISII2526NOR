using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.Web.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [MaxLength(128)]
    public string? Name { get; set; } // There is no compile-time check for nullability, so we avoid constructors (compile errors) for simplicity

    //[Required] // This attribute ensures the database column is non-nullable
    [MaxLength(256)]
    public string? Surname { get; set; } // There is no compile-time check for nullability, so we avoid constructors (compile errors) for simplicity

}


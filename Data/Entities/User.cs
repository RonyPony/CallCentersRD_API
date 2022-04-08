
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CallCentersRD_API.Database.Entities.Auth;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }


    public string Password { get; set; }

    public DateTime RegistrationDate { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime lastLogin { get; set; }
}
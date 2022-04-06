using System.ComponentModel.DataAnnotations;

namespace CallCentersRD_API.Controllers.Base.Auth.DTOs;

public class RegisterUser
{
    public string Name { get; set; }

    public string LastName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }
}
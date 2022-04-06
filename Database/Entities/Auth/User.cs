using CallCentersRD_API.Database.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace CallCentersRD_API.Database.Entities.Auth;

public class User : IdentityUser<Guid>, IBaseEntity<Guid>
{

    public string Name { get; set; }

    public string LastName { get; set; }

    public DateTime RegistrationDate { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
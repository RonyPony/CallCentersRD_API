using CallCentersRD_API.Database.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace CallCentersRD_API.Database.Entities.Auth;

public class Role : IdentityRole<Guid>, IBaseEntity<Guid>
{
    public Role()
    {

    }

    public Role(string roleName) : base(roleName)
    {
    }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
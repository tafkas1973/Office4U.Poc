using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Office4U.Domain.Model.Entities.Users
{
    public class AppRole: IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
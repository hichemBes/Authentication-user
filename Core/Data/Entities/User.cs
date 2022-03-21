using Microsoft.AspNetCore.Identity;
using System;

namespace Core.Data.Entities
{
    public class User:IdentityUser
    {
        public string filliale { get; set; }
        public Guid fk_filliale { get; set; }
    }
}

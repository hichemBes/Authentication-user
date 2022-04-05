using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Apii.ViewModels
{
    public class userRoledto
    {
        public userRoledto(IdentityUser aus, List<string> userRoles)
        {
               var   UserId = aus.Id;
              var   UserName = aus.UserName;
               var  RolesHeld = userRoles;
              var   Email = aus.Email;
              var  EmailConfirmed = aus.EmailConfirmed;
               var  LockoutEnabled = aus.LockoutEnabled;
               var  AccessFailedCount = aus.AccessFailedCount;
            
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Tinder_lvl10.Entities;

namespace API.Entities
{
    public class AppUserRole : IdentityUserRole<int> { 

    

        public AppUser User { get; set; }

        public AppRole Role { get; set; }



       
        

    }
}

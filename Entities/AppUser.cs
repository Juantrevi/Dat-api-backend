
using Dat_api.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Dat_api.Entities
{
    public class AppUser : IdentityUser <int>
    {   
        //Id, userName, PasswordHash and PasswordSalt are inherited from IdentityUser

        public DateOnly DateOfBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime LastActive { get; set; } = DateTime.UtcNow;

        public string Gender { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public List<Photo> Photos { get; set; } = new();

        public List<UserLike> LikedByUsers { get; set; }

        public List<UserLike> LikedUsers { get; set; }

        public List<Message> MessagesSent { get; set; }
        
        public List<Message> MessagesReceived { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set;}


    }
}

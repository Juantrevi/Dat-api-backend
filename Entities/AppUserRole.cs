using Microsoft.AspNetCore.Identity;

namespace Dat_api.Entities
{
	public class AppUserRole : IdentityUserRole<int>
	{

		public AppUser User { get; set; }

		public AppRole Role { get; set; }


	}
}

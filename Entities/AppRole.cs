using Microsoft.AspNetCore.Identity;

namespace Dat_api.Entities
{
	public class AppRole : IdentityRole <int>
	{

		public ICollection<AppUserRole> UserRoles { get; set; }


	}

}

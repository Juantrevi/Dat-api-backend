using Microsoft.AspNetCore.Mvc;

namespace Dat_api.Helpers
{
	public class MessageParams : PaginationParams
	{
		public string Username { get; set; }

		public string Container { get; set; } = "Unread";
	}
}

using Dat_api.DTOs;
using Dat_api.Entities;
using Dat_api.Helpers;

namespace Dat_api.Interfaces
{
	public interface ILikesRepository
	{
		Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);

		Task<AppUser> GetUserWithLikes(int userId);

		Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
	}
}

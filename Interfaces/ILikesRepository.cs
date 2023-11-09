using Dat_api.Entities;

namespace Dat_api.Interfaces
{
	public interface ILikesRepository
	{
		Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);

		Task<AppUser> GetUserWithLikes(int userId);

		Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
	}
}

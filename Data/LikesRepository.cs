using Dat_api.DTOs;
using Dat_api.Entities;
using Dat_api.Extensions;
using Dat_api.Helpers;
using Dat_api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dat_api.Data
{
	public class LikesRepository : ILikesRepository
	{
		private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
		{
			return await _context.Likes.FindAsync(sourceUserId, targetUserId);
		}

		public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
		{
			var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
			var likes = _context.Likes.AsQueryable();

			if (likesParams.Predicate == "liked")
			{
				likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
				users = likes.Select(like => like.TargetUser);
			}

			if (likesParams.Predicate == "likedBy")
			{
				likes = likes.Where(like => like.TargetUserId == likesParams.UserId);
				users = likes.Select(like => like.SourceUser);
			}

			var likedUsers = users.Select(user => new LikeDto
			{
				UserName = user.UserName,
				KnownAs = user.KnownAs,
				Age = user.DateOfBirth.calculateAge(),
				City = user.City,
				Id = user.Id,
				PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url
			});

			return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
		}

		public async Task<AppUser> GetUserWithLikes(int userId)
		{
			return await _context.Users
				.Include(x => x.LikedUsers)
				.FirstOrDefaultAsync(x => x.Id == userId);
		}
	}
}

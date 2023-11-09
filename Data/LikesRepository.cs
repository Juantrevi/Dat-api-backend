using Dat_api.DTOs;
using Dat_api.Entities;
using Dat_api.Extensions;
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

		public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
		{
			var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
			var likes = _context.Likes.AsQueryable();

			if (predicate == "liked")
			{
				likes = likes.Where(like => like.SourceUserId == userId);
				users = likes.Select(like => like.TargetUser);
			}

			if (predicate == "likedBy")
			{
				likes = likes.Where(like => like.TargetUserId == userId);
				users = likes.Select(like => like.SourceUser);
			}

			return await users.Select(user => new LikeDto
			{
				UserName = user.UserName,
				KnownAs = user.KnownAs,
				Age = user.DateOfBirth.calculateAge(),
				City = user.City,
				Id = user.Id,
				PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url
			}).ToListAsync();
		}

		public async Task<AppUser> GetUserWithLikes(int userId)
		{
			return await _context.Users
				.Include(x => x.LikedUsers)
				.FirstOrDefaultAsync(x => x.Id == userId);
		}
	}
}

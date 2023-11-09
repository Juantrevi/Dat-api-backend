﻿using Dat_api.Entities;
using Dat_api.Extensions;
using Dat_api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dat_api.Controllers
{
	public class LikesController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly ILikesRepository _likesRepository;

		public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
			_userRepository = userRepository;
			_likesRepository = likesRepository;
		}

		[HttpPost("{username}")]
		public async Task<ActionResult> AddLike(string username)
		{
			var sourceUserId =int.Parse(User.GetUserId());
			var likedUser = await _userRepository.GetUserByUserNameAsync(username);
			var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

			if (likedUser == null) return NotFound();

			if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");

			var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);

			if (userLike != null) return BadRequest("You already like this user");

			userLike = new UserLike
			{
				SourceUserId = sourceUserId,
				TargetUserId = likedUser.Id
			};

			sourceUser.LikedUsers.Add(userLike);

			if (await _userRepository.SaveAllAsync()) return Ok();

			return BadRequest("Failed to like user");
		}
    }
}

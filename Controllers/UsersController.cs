using Dat_api.Data;
using Dat_api.Entities;
using Dat_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Dat_api.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {

        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return Ok( await _userRepository.GetUsersAsync());

        
        }

        [HttpGet("{username}")]
        public async  Task<ActionResult<AppUser>> GetUser(string username)
        {

            return await _userRepository.GetUserByUserNameAsync(username);
   
        }
    }
}

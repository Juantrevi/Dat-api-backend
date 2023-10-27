using AutoMapper;
using Dat_api.Data;
using Dat_api.DTOs;
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
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();

/*            var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);*/



            return Ok(users);

        
        }

        [HttpGet("{username}")]
        public async  Task<ActionResult<MemberDto>> GetUser(string username)
        {
            //This is less performant as it needs to map the user to a memberDto
/*            var user = await _userRepository.GetUserByUserNameAsync(username);

            return _mapper.Map<MemberDto>(user);*/

            return await _userRepository.GetMemberAsync(username);
   
        }
    }
}

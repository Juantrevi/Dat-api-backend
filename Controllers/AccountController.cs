﻿using System.Security.Cryptography;
using System.Text;
using Dat_api.Data;
using Dat_api.DTOs;
using Dat_api.Entities;
using Dat_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dat_api.Controllers
{
    public class AccountController : BaseApiController
    {
     
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
         
        }

        [HttpPost("register")] //POST //api/accounts/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {   
            if(await UserExists(registerDto.Username)) return BadRequest("Username is taken");
            
            //using is used to dispose of the hmac object after it is used (Garbage Collection)
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key

            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
                //,PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };

        }

        [HttpPost("login")] //POST //api/accounts/login 
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if(user == null) return Unauthorized("Invalid username");

            //using is used to dispose of the hmac object after it is used (Garbage Collection)
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.Password[i]) return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };

        }   


        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }

    }
}

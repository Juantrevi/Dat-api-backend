using System.Security.Cryptography;
using System.Text;
using Dat_api.Data;
using Dat_api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Dat_api.Controllers
{
    public class AccountController : BaseApiController
    {
     
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
         
            _context = context;
         
        }

        [HttpPost("register")] //POST //api/accounts/register
        public async Task<ActionResult<AppUser>> Register(string username, string password)
        {   //using is used to dispose of the hmac object after it is used (Garbage Collection)
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = username,
                Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key

            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;

        }

    }
}

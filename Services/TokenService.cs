using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dat_api.Entities;
using Dat_api.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Dat_api.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim> //claims are the things we want to store inside the token
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };
        }
    }
}

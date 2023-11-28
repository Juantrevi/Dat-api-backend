using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dat_api.Entities;
using Dat_api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Dat_api.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
		private readonly UserManager<AppUser> _userManager;

		public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
			_userManager = userManager;
		}

        public  async Task<string> CreateToken(AppUser user)
        {
            var claims = new List<Claim> //claims are the things we want to store inside the token
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName) //unique name is the username
            };

            var roles = _userManager.GetRolesAsync(user).Result; //get roles of user

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role))); //add roles to claims

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); //signing credentials

            var tokenDescriptor = new SecurityTokenDescriptor //token descriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), //token expires after 7 days
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler(); //token handler

            var token = tokenHandler.CreateToken(tokenDescriptor); //create token

            return tokenHandler.WriteToken(token); //write token
        }

    }
}

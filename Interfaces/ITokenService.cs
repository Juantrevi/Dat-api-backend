using Dat_api.Entities;

namespace Dat_api.Interfaces
{
    public interface ITokenService
    {

        Task<string> CreateToken(AppUser user);

    }
}

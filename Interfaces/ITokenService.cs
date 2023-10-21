using Dat_api.Entities;

namespace Dat_api.Interfaces
{
    public interface ITokenService
    {

        string CreateToken(AppUser user);

    }
}

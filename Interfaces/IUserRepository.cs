using Dat_api.DTOs;
using Dat_api.Entities;
using Dat_api.Helpers;

namespace Dat_api.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);


        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUserByIdAsync(int id);

        Task<AppUser> GetUserByUserNameAsync(string username);

        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);

        Task<MemberDto> GetMemberAsync(string username);


    }
}

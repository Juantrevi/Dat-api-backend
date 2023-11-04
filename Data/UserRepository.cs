using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dat_api.DTOs;
using Dat_api.Entities;
using Dat_api.Helpers;
using Dat_api.Interfaces;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Dat_api.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        //Not using this method, but it's here for reference, we're using GetMembersAsync instead
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;

        }

        //Another way of getting the users, being a little bit more performant as it doesn't need to map the user to a memberDto
        public async Task<MemberDto> GetMemberAsync(string username)
        {
            //This is what it looks like without automapper
/*            return await _context.Users
                .Where(x => x.UserName == username)
                .Select(user => new MemberDto{
                Id = user.Id,
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
            }).SingleOrDefaultAsync();*/

            //This is what it looks like with automapper
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();

            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            query = query.Where(u => u.Gender == userParams.Gender);

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            return await PagedList<MemberDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<MemberDto> ( _mapper.ConfigurationProvider),
                userParams.PageNumber, 
                userParams.PageSize);
                
        }
    }
}

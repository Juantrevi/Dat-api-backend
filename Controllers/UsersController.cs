using Dat_api.Data;
using Dat_api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dat_api.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
        
            _context = context;
        
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        
        }

        [HttpGet("{id}")]
        public async  Task<ActionResult<AppUser>> GetUser(int id)
        {

            return await _context.Users.FindAsync(id);
   
        }
    }
}

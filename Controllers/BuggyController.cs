using Dat_api.Data;
using Dat_api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dat_api.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")] //GET //api/buggy/auth
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }


        [HttpGet("not-fund")] //GET //api/buggy/auth
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);

            if(thing == null) return NotFound();
            
            return thing;
        }


        [HttpGet("server-error")] //GET //api/buggy/auth
        public ActionResult<string> GetServerError()
        {

            var thing = _context.Users.Find(-1);

            var thingToReturn = thing.ToString();

                return thingToReturn;
 
        }


        [HttpGet("bad-request")] //GET //api/buggy/auth
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }

    }
}

using Dat_api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Dat_api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
     


    }
}

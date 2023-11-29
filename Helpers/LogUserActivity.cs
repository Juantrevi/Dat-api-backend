using Dat_api.Extensions;
using Dat_api.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

//This is a filter that will be applied to all the controllers, it will update the last active property of the user
namespace Dat_api.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();

            var uow = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

            var user = await uow.UserRepository.GetUserByIdAsync(userId);

            user.LastActive = DateTime.UtcNow;

            await uow.Complete();
        }


    }
}

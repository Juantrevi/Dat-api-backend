﻿using Dat_api.Extensions;
using Dat_api.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dat_api.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var username = resultContext.HttpContext.User.GetUsername();

            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

            var user = await repo.GetUserByUserNameAsync(username);

            user.LastActive = DateTime.UtcNow;

            await repo.SaveAllAsync();
        }
    }
}

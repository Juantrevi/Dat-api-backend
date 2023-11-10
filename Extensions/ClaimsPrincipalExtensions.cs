﻿using System.Security.Claims;

namespace Dat_api.Extensions
{
    //Getting the username from the token
    public static class ClaimsPrincipalExtensions
    {

        public static string GetUsername(this ClaimsPrincipal user)
        {
            //return user.FindFirst(ClaimTypes.Name)?.Value;
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }


        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

    }
}

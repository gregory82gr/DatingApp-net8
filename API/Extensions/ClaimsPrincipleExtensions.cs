using API.Data;
using API.Interfaces;
using API.Sevices;
using Microsoft.EntityFrameworkCore;
using API.Helpers;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            var username= user.FindFirstValue(ClaimTypes.Name) 
            ?? throw new Exception("Cannot get username from token");
            
            return username;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userId= int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? throw new Exception("Cannot get username from token"));
            
            return userId;
        }
    }
}
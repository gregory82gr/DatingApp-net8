using API.Extensions;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using API.Interfaces;
using API.Entities;
using API.Data;



namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Code to execute before the action executes

            var resultContext = await next(); // Proceed to the next action

            // Code to execute after the action executes

            if(context.HttpContext.User.Identity.IsAuthenticated != true) return;

            var userId = resultContext.HttpContext.User.GetUserId();
            if (userId == 0) return;
            

            var userRepository = resultContext.HttpContext.RequestServices.GetRequiredService(typeof(IUserRepository)) as IUserRepository;
            if (userRepository == null) return;

            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null) return;

            user.LastActive = DateTime.UtcNow;
            await userRepository.SaveAllAsync();// Save changes to the database   
            
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using API.Entities;
using API.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AdminController(UserManager<AppUser> userManager) : BaseApiController
{
   [Authorize(Policy = "RequireAdminRole")]
   [HttpGet("users-with-roles")]
   public async Task<ActionResult> GetUsersWithRoles()
   {
     var users= await userManager.Users
         .OrderBy(u => u.UserName)
         .Select(u => new
         {
            u.Id,
            Username=u.UserName,
            Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
         })
         .ToListAsync();

      return Ok(users);
   }

    [Authorize(Policy = "ModeratePhotoRole")]
   [HttpGet("photos-to-moderate")]
   public ActionResult GetPhotosForModeration()
   {
      return Ok("Admins or moderators can see this");
   }
}
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository) : BaseApiController
{
  
  [HttpGet]
  public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
  {
        var users= await userRepository.GetUsersAsync();

        return Ok(users);
  }

    
    [HttpGet("{username}")]
  public async  Task<ActionResult<AppUser>> GetUser(string username)
  {
        var user= await userRepository.GetUserByUsernameAsync(username);

        if(user==null) return NotFound();
        return user;
  }

}
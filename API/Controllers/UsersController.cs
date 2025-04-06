using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;
using AutoMapper;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
{
  
  [HttpGet]
  public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
  {
        var users= await userRepository.GetMembersAsync();
               

        return Ok(users);
  }

    
  [HttpGet("{username}")]
  public async  Task<ActionResult<MemberDto>> GetUser(string username)
  {
        var user= await userRepository.GetMemberAsync(username);

        if(user==null) return NotFound();
        return user;
  }

}
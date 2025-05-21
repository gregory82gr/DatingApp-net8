using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Interfaces;
using API.Sevices;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers;

public class AccountController(UserManager<AppUser> userManager,ITokenService tokenService
    , IMapper mapper) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){
        
        if(await UserExists(registerDto.Username)) return BadRequest("Username is taken");
       
       
        // var user =new AppUser
        // {
        //     UserName=registerDto.Username.ToLower(),
        //     PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
        //     PasswordSalt=hmac.Key
        // };
        var user = mapper.Map<AppUser>(registerDto);
        user.UserName=registerDto.Username.ToLower();
       
        var result=await userManager.CreateAsync(user,registerDto.Password);
        if(!result.Succeeded) return BadRequest(result.Errors);

        return new UserDto
        {
            UserName = user.UserName,
            Token = tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await userManager.Users.
        Include(x=>x.Photos)
            .FirstOrDefaultAsync(x =>
                x.NormalizedUserName == loginDto.Username.ToUpper());

        if (user == null || user.UserName==null) return Unauthorized("Invalid username");
       
       var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!result) return Unauthorized("Invalid password");

        return new UserDto
        {
            UserName = user.UserName,
            KnownAs = user.KnownAs,
            Token = tokenService.CreateToken(user),
            Gender = user.Gender,
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
        };
    }

    private async Task<bool> UserExists(string username){
        return await userManager.Users.AnyAsync(x=>x.NormalizedUserName == username.ToUpper());
    }
}
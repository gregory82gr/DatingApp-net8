using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;
using AutoMapper;
using System.Security.Claims;
using API.Interfaces;
using API.Sevices;
using API.Helpers;
using API.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;


namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper,
            IPhotoService photoService) : BaseApiController
{

      [HttpGet]
      public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
      {
            var username = User.GetUserName();
            
            userParams.CurrentUsername = username;

            var users = await userRepository.GetMembersAsync(userParams);

            Response.AddPagination(users);

            return Ok(users);
      }
      


      [HttpGet("{username}")]
      public async Task<ActionResult<MemberDto>> GetUser(string username)
      {
            var user = await userRepository.GetMemberAsync(username);

            if (user == null) return NotFound();
            return user;
      }

      [HttpPut]
      public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
      {

            var username =  User.GetUserName();

            if (username == null) return BadRequest("No usernamer found in token");

            var user = await userRepository.GetUserByUsernameAsync(username);

            if (user == null) return BadRequest("Could not find user");

            mapper.Map(memberUpdateDto, user);

            if (await userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
      }

      [HttpPost("add-photo")]
      public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
      {

            var username =  User.GetUserName();

            if (username == null) return BadRequest("No usernamer found in token");

            var user = await userRepository.GetUserByUsernameAsync(username);

            if (user == null) return NotFound("Could not find user");

            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                  Url = result.SecureUrl.AbsoluteUri,
                  PublicId = result.PublicId
            };

            if(user.Photos.Count == 0)
            {
                  photo.IsMain = true;
            }

            if (user.Photos.Count == 0) photo.IsMain = true;

            user.Photos.Add(photo);

            if (await userRepository.SaveAllAsync())
            {
                  return CreatedAtAction(nameof(GetUser),
                         new { username = user.UserName }, mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photo");
      }

      [HttpPut("set-main-photo/{photoId:int}")]
      public async Task<ActionResult> SetMainPhoto(int photoId)
      {
            var username =  User.GetUserName();

            if (username == null) return BadRequest("No usernamer found in token");

            var user = await userRepository.GetUserByUsernameAsync(username);

            if (user == null) return NotFound("Could not find user");

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null || photo.IsMain) return BadRequest("Cannot use this as  main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

            if (await userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting main photo");
      }

      [HttpDelete("delete-photo/{photoId:int}")]
      public async Task<ActionResult> DeletePhoto(int photoId)
      {
            var username =  User.GetUserName();

            if (username == null) return BadRequest("No usernamer found in token");

            var user = await userRepository.GetUserByUsernameAsync(username);

            if (user == null) return NotFound("Could not find user");

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound("Could not find photo");

            if (photo.IsMain) return BadRequest("Cannot delete main photo");

            if (photo.PublicId != null)
            {
                  var result = await photoService.DeletePhotoAsync(photo.PublicId);

                  if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting photo");
      }

}
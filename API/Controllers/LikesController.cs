using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using API.Entities;
using API.DTOs;
using System.Collections.Generic;
using API.Extensions;

namespace API.Controllers
{
    
    public class LikesController(ILikesRepository likesRepository) : BaseApiController
    {
        [HttpPost("{targetUserId:int}")]
        public async Task<ActionResult>  ToggleLike(int targetUserId)
        {
            var sourceUserId = User.GetUserId();
            if (sourceUserId == targetUserId) return BadRequest("You cannot like yourself");

            var existingLike = await likesRepository.GetUserLike(sourceUserId, targetUserId);
            if (existingLike != null)
            {
                  likesRepository.DeleteLike(existingLike);   
            }
            else
            {
                var newLike = new UserLike { SourceUserId = sourceUserId, TargetUserId = targetUserId };
                likesRepository.AddLike(newLike);
            }

            if (await likesRepository.SaveChangesAsync()) return Ok();
            
            return BadRequest("Failed to update like status");
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
        {
            return Ok(await likesRepository.GetCurrentUserLikesIds(User.GetUserId()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes(string predicate)
        {
            var userId = User.GetUserId();
            var likes = await likesRepository.GetUserLikes(predicate, userId);
            return Ok(likes);
        }

    }
   
}
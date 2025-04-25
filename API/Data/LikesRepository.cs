using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using API.Helpers;


namespace API.Data
{
    public class LikesRepository(DataContext context, IMapper mapper) : ILikesRepository
    {
        public void AddLike(UserLike userLike)
        {
            context.Likes.Add(userLike);
        }

        public void DeleteLike(UserLike userLike)
        {
            context.Likes.Remove(userLike);
        }

        public async Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<IEnumerable<int>> GetCurrentUserLikesIds(int currentUserId)
        {
            return await context.Likes
                .Where(x => x.SourceUserId == currentUserId)
                .Select(x => x.TargetUserId).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams)
        {
            var likes = context.Likes.AsQueryable();
            IQueryable<MemberDto> query;

            switch (likesParams.Predicate)
            {
                case "liked":
                    query= likes
                         .Where(x => x.SourceUserId == likesParams.UserId)
                         .Select(x => x.TargetUser)
                         .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
                         
                    break;
                case "likedBy":
                     query=  likes
                        .Where(x => x.TargetUserId == likesParams.UserId)
                        .Select(x => x.SourceUser)
                        .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
                    break; 
                default:
                    var likeIds = await GetCurrentUserLikesIds(likesParams.UserId);
                     query=  likes
                        .Where(x => x.TargetUserId == likesParams.UserId && likeIds.Contains(x.SourceUserId))
                        .Select(x => x.SourceUser)
                        .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
                        
                    break;
            }
            return await PagedList<MemberDto>.CreateAsync(query,likesParams.PageNumber,likesParams.PageSize);
            
        }

        
    }
}
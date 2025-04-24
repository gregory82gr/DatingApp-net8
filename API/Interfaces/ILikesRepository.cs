using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using AutoMapper;
using API.DTOs;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        // Define method signatures here
        Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);
        Task<IEnumerable<MemberDto>> GetUserLikes(string predicate,int userId);
        Task<IEnumerable<int>> GetCurrentUserLikesIds(int currentUserId);
        void AddLike(UserLike userLike);
        void DeleteLike(UserLike userLike);
        Task<bool> SaveChangesAsync();
    }
}
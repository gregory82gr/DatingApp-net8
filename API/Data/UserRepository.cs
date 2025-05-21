using API.Entities;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using API.DTOs;
using API.Helpers;
using API.Extensions;
namespace API.Data;


public class UserRepository(DataContext _context,IMapper mapper) : IUserRepository
{
    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.
             Include(x => x.Photos).
            SingleOrDefaultAsync(x => x.NormalizedUserName == username.ToUpper());
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _context.Users
        .Include(x=>x.Photos).
        ToListAsync();
    }
    public async Task<PagedList<MemberDto>> GetMembersAsync( UserParams userParams)
    {
      
        var query= _context.Users.AsQueryable();
        query = query.Where(x => x.UserName != userParams.CurrentUsername);
        if(userParams.Gender != null)
        {
            query = query.Where(x => x.Gender == userParams.Gender);
        }


        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));
        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x => x.LastActive)
        };
        
        
        var projectedQuery = query.ProjectTo<MemberDto>(mapper.ConfigurationProvider);
        
        return await PagedList<MemberDto>.CreateAsync(projectedQuery, userParams.PageNumber, userParams.PageSize);
            
    }
    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        return await _context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }
}

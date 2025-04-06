using API.Entities;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace API.Data;


public class UserRepository(DataContext _context) : IUserRepository
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
            SingleOrDefaultAsync(x => x.UserName == username.ToLower());
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _context.Users
        .Include(x=>x.Photos).
        ToListAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Interfaces;
using System.Text.Json;
using System.Collections.Generic;

namespace API.Data{

    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return; // DB has been seeded

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            // Deserialize the JSON data into a list of AppUser objects 
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            if (users == null) return;

            foreach (var user in users)
            {
                using var hmac= new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;
                await context.Users.AddAsync(user);
            }

            await context.SaveChangesAsync();
        }
    }

}
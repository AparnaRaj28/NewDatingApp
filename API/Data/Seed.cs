using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
          if(await context.Users.AnyAsync()) return; //checks if we have any users in the database.
          //seeding the data if we do not have any users in the database.
          var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
          //if there is a mistake in casing in the seed data
          var options= new JsonSerializerOptions{PropertyNameCaseInsensitive = true};
          //json into a c# object.Deserialising to a list of AppUser
          var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
          //generating passwords
          foreach(var user in users)
          {
            using var hmac = new HMACSHA512();
            user.UserName = user.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt = hmac.Key;
            
            //adding to the entity framework
            context.Users.Add(user);
          }
          await context.SaveChangesAsync();
        }
    }
}
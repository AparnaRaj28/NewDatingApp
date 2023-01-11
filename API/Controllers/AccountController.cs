using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Interface;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace API.Controllers
{
   
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context,ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
            
        }
         [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegsiterDto registerData)
        {
            if( await userExists(registerData.UserName)){ return BadRequest("Username is taken");}
            using var hmac = new HMACSHA512();
             var user = new AppUser{
                UserName = registerData.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerData.Password)),
                PasswordSalt = hmac.Key
             };

             _context.Users.Add(user);
             await _context.SaveChangesAsync();

             return new UserDto{
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
             };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){

            var user = await _context.Users.SingleOrDefaultAsync(x=>x.UserName == loginDto.UserName);
            
            // return await user;           
            if(user == null) return Unauthorized("No such username");

            using var hmac = new HMACSHA512(user.PasswordSalt); // the key is passed to get the exact same hashing algorithm//this returns a byte array

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            
            //compare the computed hash and the user password hash
            for(int i=0; i<computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("invalid password");
                }
            }
             return new UserDto{
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
             };
        }
        

        private async Task<bool> userExists(string username){
           return await _context.Users.AnyAsync(x=>x.UserName == username.ToLower());
        }
    }
}
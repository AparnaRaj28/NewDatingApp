using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Interface;
using Entities;

using Microsoft.IdentityModel.Tokens;

namespace API.Service
{
    public class TokenService : ITokenService
    {
        //same key that is used for encryption as well as decryption
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            //ssk takes a byte array and specify the config which takes a key
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user)
        {
           var claims  = new List<Claim>
           {
             new Claim(JwtRegisteredClaimNames.NameId,user.UserName)
           };

           //signing credentials
           var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
           var tokenDescriptor = new SecurityTokenDescriptor
           {
             Subject = new ClaimsIdentity(claims),
             Expires = DateTime.Now.AddDays(7),
             SigningCredentials = creds
           };
           var tokenHandler = new JwtSecurityTokenHandler();
           var token = tokenHandler.CreateToken(tokenDescriptor);
           return tokenHandler.WriteToken(token);
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature
{
    internal class GenerateTokens
    {
       
       
        public async static Task<JwtSecurityToken> GenerateAccessToken(ApplicationUser user, UserManager<ApplicationUser> userManager,IConfiguration configuration)
        {
            var cliams = new List<Claim>();

            cliams.Add(new Claim(ClaimTypes.Name, user.UserName));
            cliams.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            cliams.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));

            var Roles = await userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                cliams.Add(new Claim(ClaimTypes.Role, role));
            }

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTAuth:SecretKey"]));
            var SecretKey = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(

                claims: cliams,
                issuer: configuration["JWTAuth:Issuer"],
                audience: configuration["JWTAuth:Audience"],
                expires: DateTime.UtcNow.AddDays(double.Parse(configuration["JWTAuth:DurationExpire"])),
                signingCredentials: SecretKey
                );

            return token;
        }
    }
}

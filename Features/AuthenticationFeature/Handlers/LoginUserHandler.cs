using DataAccess.Repositry.IRepositry;
using Features.AuthenticationFeature.Quieries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Handlers
{
    public class LoginUserHandler : IRequestHandler<LogInUserCommand, AuthDTO>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor http;
        private readonly IRefreshTokenRepositry refreshTokenRepositry;

        public LoginUserHandler(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IHttpContextAccessor http,
            IRefreshTokenRepositry refreshTokenRepositry
            )
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.http = http;
            this.refreshTokenRepositry = refreshTokenRepositry;
        }
        public async Task<AuthDTO> Handle(LogInUserCommand request, CancellationToken cancellationToken)
        {
            var userLogin = request.User;

            var user = await userManager.FindByEmailAsync(userLogin.Email);
            if (user is not null)
            {
                var checking = await userManager.CheckPasswordAsync(user, userLogin.Password);
                if (checking)
                {
                    // Token Generate 

                    var token=await GenerateTokens.GenerateAccessToken(user,userManager,configuration);
                    var _token = new JwtSecurityTokenHandler().WriteToken(token);

                    var refreshToken = new RefreshToken
                    {
                        TokenHash = Guid.NewGuid().ToString("N"),
                        Created = DateTime.UtcNow,
                        Expired = DateTime.UtcNow.AddDays(1),
                        IsRevoked = false,
                        IsUsed = false,
                        UserID = user.Id,
                        JwtId = token.Id,
                        IpAddress = http.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                        DeviceInfo = $"{http.HttpContext?.Request.Headers["User-Agent"]}"
                    };

                    await refreshTokenRepositry.AddAsync(refreshToken);
                    await refreshTokenRepositry.CommitAsync(cancellationToken);

                    return new AuthDTO
                    {
                        AccessToken = _token,
                        AccessTokenExpiration = token.ValidTo,
                        RefreshToken=refreshToken.TokenHash,
                        RefreshTokenExpiration=refreshToken.Expired,
                        
                    };
                }
                return new AuthDTO
                {
                    Message = "The Password Is Invalid"
                };
            }   
            return new AuthDTO
            {
                    Message = "The User Not Found"
            };
        }
    }
}

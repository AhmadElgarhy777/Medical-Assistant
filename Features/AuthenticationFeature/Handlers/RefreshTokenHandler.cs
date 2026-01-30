using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using Features.AuthenticationFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Handlers
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthDTO>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IRefreshTokenRepositry refreshTokenRepositry;

        public RefreshTokenHandler(UserManager<ApplicationUser> userManager,IConfiguration configuration,IRefreshTokenRepositry refreshTokenRepositry)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.refreshTokenRepositry = refreshTokenRepositry;
        }
        public async Task<AuthDTO> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenHash = request.RefreshToken;
            var IpAddress=request.IpAddress;
            var DeviceInfo=request.DeviceInfo;

            var spec = new RefreshTokenSpecifcation(tokenHash);
            var StoredRefreshToken = refreshTokenRepositry.GetOne(spec).FirstOrDefault();

            if(StoredRefreshToken == null|| StoredRefreshToken.IsUsed|| StoredRefreshToken.IsRevoked)
            {
                return new AuthDTO
                {
                    Message = "Invalid or expired refresh token"
                };
            }
            if(StoredRefreshToken.IpAddress != IpAddress ||StoredRefreshToken.DeviceInfo != DeviceInfo)
            {
                return new AuthDTO
                {
                    Message = "Token used from unauthorized device or IP"
                };
            }

            StoredRefreshToken.IsUsed=true;
            StoredRefreshToken.IsRevoked=true;
            StoredRefreshToken.ReplacedByTokenHash = HashToken(tokenHash);

            var user = StoredRefreshToken.User;

            var NewAccessToken = await GenerateTokens.GenerateAccessToken(user, userManager, configuration);
            var _newAccessToken = new JwtSecurityTokenHandler().WriteToken(NewAccessToken);

            var newrefreshtoken = GenerateRefreshToken(user, IpAddress, DeviceInfo);

            await refreshTokenRepositry.AddAsync(newrefreshtoken);
            await refreshTokenRepositry.CommitAsync(cancellationToken);

            return new AuthDTO
            {
                AccessToken = _newAccessToken,
                AccessTokenExpiration = NewAccessToken.ValidTo,
                RefreshToken = newrefreshtoken.TokenHash,
                RefreshTokenExpiration = newrefreshtoken.Expired
            };


        }
        private RefreshToken GenerateRefreshToken(ApplicationUser user, string ipAddress, string deviceInfo)
        {
            var refreshToken = new RefreshToken
            {
                TokenHash = HashToken(Guid.NewGuid().ToString("N")),
                JwtId = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow,
                Expired = DateTime.UtcNow.AddDays(7),
                UserID = user.Id,
                IpAddress = ipAddress,
                DeviceInfo = deviceInfo
            };

            return refreshToken;
        }

        private string HashToken(string token) => Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
    }
}

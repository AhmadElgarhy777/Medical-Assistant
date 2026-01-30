using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using Features.AuthenticationFeature.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Handlers
{
    public class LogOutHandler : IRequestHandler<LogOutCommand, bool>
    {
        private readonly IRefreshTokenRepositry refreshTokenRepositry;

        public LogOutHandler(IRefreshTokenRepositry refreshTokenRepositry)
        {
            this.refreshTokenRepositry = refreshTokenRepositry;
        }
        public async Task<bool> Handle(LogOutCommand request, CancellationToken cancellationToken)
        {
            var tokenHash = request.RefreshToken;
            var IpAddress = request.IpAddress;
            var DeviceInfo = request.DeviceInfo;

            var spec = new RefreshTokenSpecifcation(tokenHash);
            var StoredRefreshToken = refreshTokenRepositry.GetOne(spec).FirstOrDefault();

            if (StoredRefreshToken == null || StoredRefreshToken.IpAddress != IpAddress || StoredRefreshToken.DeviceInfo != DeviceInfo)
                return false;

            StoredRefreshToken.IsRevoked = true;
            refreshTokenRepositry.Edit(StoredRefreshToken);
           await refreshTokenRepositry.CommitAsync(cancellationToken);
            return true;    
        }
    }
}

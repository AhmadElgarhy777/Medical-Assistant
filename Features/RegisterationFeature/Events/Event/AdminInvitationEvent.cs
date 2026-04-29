using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Events.Event
{
    public record AdminInvitationEvent(string UserId, string Email, string UserName, string EmailConfirmToken, string SetPasswordToken, CancellationToken CancellationToken):INotification;
  
}

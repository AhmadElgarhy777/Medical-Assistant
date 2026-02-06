using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Events.Event
{
    public record ConfirmEmailEvent(string UserId, string Email,CancellationToken CancellationToken, string Type = "link") :INotification;
}

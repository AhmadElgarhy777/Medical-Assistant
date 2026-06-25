using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TwilioProviderServices.WhatsUp
{
    public interface ISmsService
    {
        Task SendOtpAsync(string toPhone, string message);
    }
}

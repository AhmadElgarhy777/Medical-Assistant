using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Services.TwilioProviderServices.WhatsUp
{
    public class TwilioWhatsAppService : ISmsService
    {
        private readonly TwilioSettingsModel settings;

        public TwilioWhatsAppService(IOptions<TwilioSettingsModel> options)
        {
            settings = options.Value;
            TwilioClient.Init(settings.AccountSid, settings.AuthToken);
        }

        public async Task SendOtpAsync(string toPhone, string message)
        {
            // toPhone must be in international format e.g. +201234567890
            await MessageResource.CreateAsync(
                to: new PhoneNumber($"whatsapp:{toPhone}"),
                from: new PhoneNumber(settings.WhatsAppFrom),
                body: message);
        }
    }
}

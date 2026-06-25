using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TwilioProviderServices.WhatsUp
{
    public class TwilioSettingsModel
    {
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
        public string WhatsAppFrom { get; set; }
    }
}

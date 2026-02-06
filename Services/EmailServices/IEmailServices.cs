using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailServices
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string to, string Subject, string body, CancellationToken cancellationToken=default);
    }
}

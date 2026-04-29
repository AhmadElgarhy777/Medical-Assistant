using Features.RegisterationFeature.Events.Event;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Models;
using Services.EmailServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Events.Handlers
{
    public class AdminInvitationEventHandler : INotificationHandler<AdminInvitationEvent>
    {
        private readonly IEmailServices emailServices;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        // ⚠️ Put your frontend base URL in appsettings.json and inject IConfiguration
        //private const string FrontendBaseUrl = "https://yourfrontend.com";

        public AdminInvitationEventHandler(
            IEmailServices emailServices,
            UserManager<ApplicationUser> userManager,IConfiguration  configuration)
        {
            this.emailServices = emailServices;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task Handle(AdminInvitationEvent notification, CancellationToken cancellationToken)
        {
            // ── Encode tokens so they are safe in a URL ─────────────────────────
            var encodedEmailToken = WebEncoders.Base64UrlEncode(
                                           Encoding.UTF8.GetBytes(notification.EmailConfirmToken));

            var encodedPasswordToken = WebEncoders.Base64UrlEncode(
                                           Encoding.UTF8.GetBytes(notification.SetPasswordToken));

            // ── Build the single invitation link ────────────────────────────────
            // One link does both: confirm email + redirect to set-password page
            var invitationLink = $"{configuration["FrontendBaseUrl"]}/admin/accept-invitation" +
                                 $"?userId={notification.UserId}" +
                                 $"&emailToken={encodedEmailToken}" +
                                 $"&passwordToken={encodedPasswordToken}";

            // ── Build HTML email body ───────────────────────────────────────────
            var emailBody = $@"
                <h2>Welcome, {notification.UserName}!</h2>
                <p>You have been added as an <strong>Admin</strong>.</p>
                <p>Please click the button below to confirm your email and set your password:</p>
                <br/>
                <a href='{invitationLink}'
                   style='background:#4F46E5;color:white;padding:12px 24px;
                          border-radius:6px;text-decoration:none;font-weight:bold;'>
                    Accept Invitation
                </a>
                <br/><br/>
                <p style='color:gray;font-size:12px;'>
                    This link will expire in 24 hours. If you did not expect this email, ignore it.
                </p>";

            // ── Send the email ──────────────────────────────────────────────────
            await emailServices.SendEmailAsync(
                to: notification.Email,
                Subject: "You're invited as Admin — Set your password",
                body: emailBody);
        }
    }
}

﻿using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace MVC5_R.WebApp.Infrastructure.Sms
{
    public class NoopSmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
﻿using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace MVC5_R.WebApp.Features.Account
{
    public class LogOff
    {
        public class Command : IRequest
        {
        }

        public class Handler : IAsyncRequestHandler<Command>
        {
            private readonly IAuthenticationManager _authenticationManager;

            public Handler(IAuthenticationManager authenticationManager)
            {
                _authenticationManager = authenticationManager;
            }

            public async Task Handle(Command command)
            {
                await Task.Factory.StartNew(() =>
                {
                    _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                });
            }
        }
    }
}
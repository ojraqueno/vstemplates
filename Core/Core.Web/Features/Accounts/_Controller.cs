using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core.Web.Features.Accounts
{
    [AllowAnonymous]
    public class AccountsController : Controller
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
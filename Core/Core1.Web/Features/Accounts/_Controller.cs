using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core1.Web.Features.Accounts
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("api/accounts/login")]
        public async Task<IActionResult> Login(Login.Command command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            return Ok();
        }

        [HttpPost("api/account/createToken")]
        public async Task<IActionResult> CreateToken(CreateToken.Command command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            return Created("", result);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("api/accounts/register")]
        public async Task<IActionResult> Register([FromBody] Register.Command command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            return Ok();
        }
    }
}
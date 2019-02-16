using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core1.Web2.Features.Accounts
{
    public class AccountsController : Controller
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmail.Command command)
        {
            var result = await _mediator.Send(command);

            return View(result);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("api/accounts/forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword.Command command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl, bool? fromRegistration)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.FromRegistration = fromRegistration;

            return View();
        }

        [HttpPost("api/accounts/login")]
        public async Task<IActionResult> Login([FromBody] Login.Command command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(Logout.Command command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("api/accounts/createToken")]
        public async Task<IActionResult> CreateToken([FromBody] CreateToken.Command command)
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

        [HttpGet]
        public async Task<IActionResult> ResetPassword(ResetPassword.Query query)
        {
            var result = await _mediator.Send(query);

            return View(result);
        }

        [HttpPost("api/accounts/resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword.Command command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
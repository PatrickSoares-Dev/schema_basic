using Microsoft.AspNetCore.Mvc;
using swagger_basic.Models.Token;
using swagger_basic.Models.Users;
using swagger_basic.Services;
using swagger_basic.Services.IServices;
using System.Threading.Tasks;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    /// <summary>
    /// Autentica um usuário e retorna um token de acesso.
    /// </summary>
    /// <param name="user">As credenciais do usuário para login.</param>
    /// <returns>Um objeto contendo o token de acesso.</returns>
    /// <response code="200">Login bem-sucedido.</response>
    /// <response code="400">Erro ao receber os dados da solicitação.</response>
    /// <response code="401">Usuário ou senha inválidos.</response>
    [ProducesResponseType(typeof(UserTokenModels), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(object), 401)]
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginModel user)
    {
        if (user == null)
        {
            return BadRequest("Aconteceu um erro ao receber os dados da sua solicitação.");
        }

        var loginResult = _authService.LoginUser(user);

        if (loginResult is UnauthorizedObjectResult)
        {
            return Unauthorized(new { message = "Usuário ou senha inválidos." });
        }

        var token = _authService.GenerateToken(user.Email);
        return Ok(new UserTokenModels { Token = token });
    }


}

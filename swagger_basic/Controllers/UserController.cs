using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using swagger_basic.Entities;
using swagger_basic.Models.Users;
using swagger_basic.Repositories.IRepository;
using swagger_basic.Services;
using swagger_basic.Services.IServices;

[Route("user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UserController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    /// <summary>
    /// Cria um novo usuário.
    /// </summary>
    /// <param name="user">Informações do novo usuário.</param>
    /// <returns>ActionResult indicando o resultado da operação.</returns>
    [HttpPost("create")]
    public IActionResult Register([FromBody] User user)
    {
        if (user == null)
        {
            return BadRequest("Aconteceu um erro ao receber os dados da sua solicitação.");
        }

        return _userService.CreateUser(user);
    }

    /// <summary>
    /// Obtém todos os usuários (requer autenticação).
    /// </summary>
    /// <returns>ActionResult contendo a lista de usuários.</returns>
    [Authorize]
    [HttpGet("all")]
    public IActionResult GetAllUsers()
    {
        var permissao = 0;

        if (!_authService.VerificarPermissao(User, permissao))
        {
            return Unauthorized("Você não tem permissão para fazer isso.");
        }

        return _userService.GetAllUsers();
    }


    /// <summary>
    /// Obtém um usuário pelo ID (requer autenticação).
    /// </summary>
    /// <param name="userId">ID do usuário a ser obtido.</param>
    /// <returns>ActionResult contendo as informações do usuário.</returns>
    [Authorize]
    [HttpGet("{userId}")]
    public IActionResult GetUserById(int userId)
    {

        if (userId == null)
        {
            return BadRequest("Aconteceu um erro ao receber os dados da sua solicitação.");
        }

        if (!_authService.VerificarPermissao(User, userId))
        {
            return Unauthorized("Você não tem permissão para fazer isso.");
        }

        return _userService.GetUserById(userId);
    }

    /// <summary>
    /// Atualiza informações de um usuário (requer autenticação).
    /// </summary>
    /// <param name="updatedFields">Campos atualizados do usuário.</param>
    /// <returns>ActionResult indicando o resultado da operação.</returns>
    [Authorize]
    [HttpPut("update")]
    public IActionResult UpdateUser([FromBody] UserUpdateModel updatedFields)
    {
        if (updatedFields == null)
        {
            return BadRequest("Aconteceu um erro ao receber os dados da sua solicitação.");
        }

        if (!_authService.VerificarPermissao(User, updatedFields.Id))
        {
            return Unauthorized("Você não tem permissão para fazer isso.");
        }

        return _userService.UpdateUser(updatedFields);
    }


    /// <summary>
    /// Remove um usuário pelo ID (requer autenticação).
    /// </summary>
    /// <param name="userId">ID do usuário a ser removido.</param>
    /// <returns>ActionResult indicando o resultado da operação.</returns>
    [Authorize]
    [HttpDelete("remove/{userId}")]
    public IActionResult RemoveUser(int userId)
    {
        if (userId == null)
        {
            return BadRequest("Aconteceu um erro ao receber os dados da sua solicitação.");
        }

        if (!_authService.VerificarPermissao(User, userId))
        {
            return Unauthorized("Você não tem permissão para fazer isso.");
        }

        return _userService.RemoveUser(userId);
    }
}

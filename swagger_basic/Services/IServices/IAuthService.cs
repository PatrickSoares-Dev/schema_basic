using Microsoft.AspNetCore.Mvc;
using swagger_basic.Models.Users;
using System.Security.Claims;

namespace swagger_basic.Services.IServices
{
    public interface IAuthService
    {
        IActionResult LoginUser(UserLoginModel user);
        public string GenerateToken(string email);
        bool VerificarPermissao(ClaimsPrincipal userClaims, int userIdToOperate);
    }
}

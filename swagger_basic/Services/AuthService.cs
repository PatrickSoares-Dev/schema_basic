using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using swagger_basic.Entities;
using swagger_basic.Models.Users;
using swagger_basic.Repositories.IRepository;
using swagger_basic.Services.IServices;
using swagger_basic.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace swagger_basic.Services
{
    public class AuthService : IAuthService
    {

        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, ITokenRepository tokenRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _configuration = configuration;
        }

        public IActionResult LoginUser(UserLoginModel user)
        {
            var existingUser = _userRepository.GetUserByIdOrEmail(user.Email);

            if (existingUser == null || existingUser.Senha != user.Senha.GerarHash())
            {
                return new UnauthorizedObjectResult("Credenciais inválidas");
            }

            return new OkObjectResult("Login bem-sucedido");
        }

        public bool VerificarPermissao(ClaimsPrincipal userClaims, int userIdToOperate)
        {
            var userId = int.Parse(userClaims.FindFirst("id").Value);

            // Se o usuário estiver tentando na sua própria conta, permita
            if (userId == userIdToOperate)
            {
                return true;
            }

            var user = _userRepository.GetUserByIdOrEmail(userId);
            return user != null && user.Tipo_User == "Administrador";
        }

        public string GenerateToken(string email)
        {
            var user = _userRepository.GetUserByIdOrEmail(email);

            var existingToken = _tokenRepository.GetValidTokenByUserId(user.Id);

            if (existingToken != null)
            {
                return existingToken.Token;
            }

            var expiredToken = _tokenRepository.GetExpiredTokenByUserId(user.Id);

            if (expiredToken != null)
            {
                return "Token expirado";
            }

            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("email", email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:secretKey"]));
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(180);

            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, ToUnixEpochDate(expiration).ToString(), ClaimValueTypes.Integer));

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var authToken = new AuthToken
            {
                User_Id = user.Id,
                Token = tokenString,
                Expiration_Date = expiration,
                Generation_Date = DateTime.UtcNow
            };

            _tokenRepository.AddToken(authToken);

            return tokenString;
        }



        private static long ToUnixEpochDate(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }

    }
}

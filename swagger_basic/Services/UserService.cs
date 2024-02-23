using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using swagger_basic.Data;
using swagger_basic.Entities;
using swagger_basic.Models.Users;
using swagger_basic.Repositories.IRepository;
using swagger_basic.Services.IServices;
using swagger_basic.Utilities;
using System;
using System.Linq;

namespace swagger_basic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult CreateUser(User user)
        {

            if (_userRepository.GetUserByIdOrEmail(user.Email) != null)
            {
                return new ConflictObjectResult("Este e-mail já está registrado.");
            }

            user.Data_Registro = DateTime.UtcNow;
            user.Tipo_User = string.IsNullOrEmpty(user.Tipo_User) ? "Usuario" : user.Tipo_User;

            try
            {
                _userRepository.AddUser(user);
                return new OkObjectResult("Usuário registrado com sucesso.");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Erro interno do servidor: {ex.Message}")
                {
                    StatusCode = 500
                };
            }
        }

        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            var totalUsers = users.Count;

            var responseUsers = users.Select(u => new UserResponseModel
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Data_Registro = u.Data_Registro,
                Tipo_User = u.Tipo_User
            }).ToList();

            var userListResponse = new UserListResponseModel
            {
                Users = responseUsers,
                TotalUsers = totalUsers
            };

            return new OkObjectResult(userListResponse);
        }


        public IActionResult GetUserById(int userId)
        {
            var user = _userRepository.GetUserByIdOrEmail(userId);

            if (user == null)
            {
                return new NotFoundObjectResult("Usuário não encontrado.");
            }

            var responseUser = new UserResponseModel
            {
                Id = user.Id,
                Nome = user.Nome,
                Email = user.Email,
                Data_Registro = user.Data_Registro,
                Tipo_User = user.Tipo_User
            };

            return new OkObjectResult(responseUser);
        }


        public IActionResult UpdateUser(UserUpdateModel updatedFields)
        {            
            var existingUser = _userRepository.GetUserByIdOrEmail(updatedFields.Id);

            if (existingUser == null)
            {
                return new NotFoundObjectResult("Usuário não encontrado.");
            }

            if (!string.IsNullOrEmpty(updatedFields.Nome))
            {
                existingUser.Nome = updatedFields.Nome;
            }

            if (!string.IsNullOrEmpty(updatedFields.Email))
            {
                existingUser.Email = updatedFields.Email;
            }

            if (!string.IsNullOrEmpty(updatedFields.Senha))
            {
                existingUser.Senha = updatedFields.Senha.GerarHash();
            }

            _userRepository.UpdateUser(existingUser);

            return new OkObjectResult("Usuário atualizado com sucesso.");
        }

        public IActionResult RemoveUser(int userId)
        {
            var existingUser = _userRepository.GetUserByIdOrEmail(userId);

            if (existingUser == null)
            {
                return new NotFoundObjectResult("Usuário não encontrado.");
            }            

            _userRepository.RemoveUser(userId);

            return new OkObjectResult("Usuário removido com sucesso.");
        }

    }
}

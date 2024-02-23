using Microsoft.AspNetCore.Mvc;
using swagger_basic.Entities;
using swagger_basic.Models.Users;

namespace swagger_basic.Services.IServices
{
    public interface IUserService
    {
        IActionResult CreateUser(User user);
        IActionResult GetAllUsers();
        IActionResult GetUserById(int userId);
        IActionResult UpdateUser(UserUpdateModel updatedFields);
        IActionResult RemoveUser(int userId);
    }
}

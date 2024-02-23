using Microsoft.AspNetCore.Mvc;
using swagger_basic.Entities;

namespace swagger_basic.Repositories.IRepository
{
    public interface IUserRepository
    {
        User GetUserByIdOrEmail(object identifier);
        List<User> GetAllUsers();
        void AddUser(User user);
        void RemoveUser(object identifier);
        void UpdateUser(User updatedFields);
    }
}

using swagger_basic.Data;
using swagger_basic.Entities;
using swagger_basic.Repositories.IRepository;
using swagger_basic.Utilities;

namespace swagger_basic.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList(); 
        }

        public User GetUserByIdOrEmail(object identifier)
        {
            if (identifier is int userId)
            {
                return _context.Users.Find(userId);
            }
            else if (identifier is string userEmail)
            {
                return _context.Users.FirstOrDefault(u => u.Email.ToLower() == userEmail.ToLower());
            }

            return null;
        }

        public void AddUser(User user)
        {
            user.SetSenhaHash();
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void RemoveUser(object identifier)
        {
            var userToRemove = _context.Users.FirstOrDefault(u => u.Id == (int)identifier || u.Email == identifier.ToString());

            if (userToRemove != null)
            {
                _context.Users.Remove(userToRemove);
                _context.SaveChanges();
            }
        }

        public void UpdateUser(User updatedFields)
        {
            var existingUser = GetUserByIdOrEmail(updatedFields.Id);

            if (existingUser != null)
            {
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

                if (!string.IsNullOrEmpty(updatedFields.Tipo_User))
                {
                    existingUser.Tipo_User = updatedFields.Tipo_User;
                }

                _context.SaveChanges();
            }
        }

    }
}

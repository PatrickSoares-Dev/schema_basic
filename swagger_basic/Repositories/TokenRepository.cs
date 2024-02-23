using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using swagger_basic.Data;
using swagger_basic.Entities;
using swagger_basic.Repositories.IRepository;

namespace swagger_basic.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly DataContext _context;

        public TokenRepository(DataContext context)
        {
            _context = context;
        }

        public AuthToken GetValidTokenByUserId(int userId)
        {
            return _context.Auth_Tokens
                .Where(t => t.User_Id == userId && t.Expiration_Date > DateTime.UtcNow)
                .OrderByDescending(t => t.Expiration_Date)
                .FirstOrDefault();
        }

        public AuthToken GetExpiredTokenByUserId(int userId)
        {
            return _context.Auth_Tokens
                .Where(t => t.User_Id == userId && t.Expiration_Date <= DateTime.UtcNow)
                .OrderByDescending(t => t.Expiration_Date)
                .FirstOrDefault();
        }

        public void AddToken(AuthToken authToken)
        {
            _context.Auth_Tokens.Add(authToken);
            _context.SaveChanges();
        }
    }
}

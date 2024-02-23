using swagger_basic.Entities;

namespace swagger_basic.Repositories.IRepository
{
    public interface ITokenRepository
    {

        AuthToken GetValidTokenByUserId(int userId);
        AuthToken GetExpiredTokenByUserId(int userId);
        void AddToken(AuthToken authToken);
    }

}

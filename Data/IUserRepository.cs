using dotnet_auth.Models;

namespace dotnet_auth.Data
{
    public interface IUserRepository
    {
        User Create(User user);
        User GetByEmail(string email);
        User GetById(int id);
    }
}
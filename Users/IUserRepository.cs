using husarbeid.Data;

namespace husarbeid.Users
{
    public interface IUserRepository
    {
        User GetUserById(string id);
    }
}
using husarbeid.Data;

namespace husarbeid.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetUserById(string id)
        {
            return _context.Users.Find(id);
        }
    }
}
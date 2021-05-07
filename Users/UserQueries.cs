using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using husarbeid.Data;
using husarbeid.DataLoader;

namespace husarbeid.Users
{
    [ExtendObjectType(name: "Query")]
    public class UserQueries
    {
        [UseApplicationDbContext]
        public Task<List<User>> GetUsers([ScopedService] ApplicationDbContext context) =>
            context.Users.ToListAsync();

        public Task<User> GetUserAsync(
            [ID(nameof(User))] int id,
            UserByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);
    }
}
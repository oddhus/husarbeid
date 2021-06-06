using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using husarbeid.Data;
using husarbeid.DataLoader;
using husarbeid.Common;

namespace husarbeid.Users
{
    [ExtendObjectType(name: "Query")]
    public class UserQueries
    {
        [UseApplicationDbContext]
        public Task<List<User>> GetUsers([ScopedService] ApplicationDbContext context) =>
            context.Users.ToListAsync();

        public async Task<GetUserPayload> FindUserAsync(
            [ID(nameof(User))] int? id,
            [GlobalStateAttribute("currentUserId")] int? currentUserId,
            UserByIdDataLoader dataLoader,
            CancellationToken cancellationToken)
        {
            if (currentUserId == null)
            {
                return new GetUserPayload(
                    new UserError("Please sign in and try again", "NOT_AUTHORIZED"));
            }

            User signedInUser = await dataLoader.LoadAsync(currentUserId.GetValueOrDefault(), cancellationToken);

            User? foundUser = null;

            if (id != null)
            {
                foundUser = await dataLoader.LoadAsync(id.GetValueOrDefault(), cancellationToken);
            }

            if (foundUser != null && (foundUser.FamilyId != signedInUser.FamilyId))
            {
                return new GetUserPayload(
                    new UserError("Not allowed to view other families", "NOT_AUTHORIZED"));
            }


            return new GetUserPayload(foundUser ?? signedInUser);
        }
    }
}
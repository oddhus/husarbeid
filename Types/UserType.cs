using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using husarbeid.Data;
using husarbeid.DataLoader;

namespace husarbeid.Types
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<UserByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor.Field(u => u.HashedPassword).Ignore();

            descriptor
                .Field(u => u.UserTasks)
                .ResolveWith<UserResolvers>(t => t.GetFamilyTasksAsync(default!, default!, default!, default))
                .UseDbContext<ApplicationDbContext>();

            descriptor
                .Field(u => u.Family)
                .ResolveWith<UserResolvers>(t => t.GetFamilyAsync(default!, default!, default!));
        }

        private class UserResolvers
        {
            public async Task<IEnumerable<FamilyTask>> GetFamilyTasksAsync(
                User user,
                [ScopedService] ApplicationDbContext dbContext,
                FamilyTaskByIdDataLoader taskById,
                CancellationToken cancellationToken)
            {
                int[] taskIds = await dbContext.Users
                    .Where(s => s.Id == user.Id)
                    .Include(s => s.UserTasks)
                    .SelectMany(s => s.UserTasks.Select(t => t.Id))
                    .ToArrayAsync();

                return await taskById.LoadAsync(taskIds, cancellationToken);
            }


            public async Task<Family?> GetFamilyAsync(
                User user,
                FamilyByIdDataLoader familyById,
                CancellationToken cancellationToken)
            {
                if (user.FamilyId is null)
                {
                    return null;
                }

                return await familyById.LoadAsync(user.FamilyId.Value, cancellationToken);
            }


        }
    }
}
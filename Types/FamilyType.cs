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
    public class FamilyType : ObjectType<Family>
    {
        protected override void Configure(IObjectTypeDescriptor<Family> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<FamilyByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(f => f.Members)
                .ResolveWith<FamilyResolvers>(t => t.GetMembersAsync(default!, default!, default!, default))
                .UseDbContext<ApplicationDbContext>();


            descriptor
                .Field(t => t.Tasks)
                .ResolveWith<FamilyResolvers>(t => t.GetFamilyTasksAsync(default!, default!, default!, default))
                .UseDbContext<ApplicationDbContext>();
        }

        private class FamilyResolvers
        {
            public async Task<IEnumerable<FamilyTask>> GetFamilyTasksAsync(
                Family family,
                [ScopedService] ApplicationDbContext dbContext,
                FamilyTaskByIdDataLoader taskById,
                CancellationToken cancellationToken)
            {
                int[] taskIds = await dbContext.Families
                    .Where(s => s.Id == family.Id)
                    .Include(s => s.Tasks)
                    .SelectMany(s => s.Tasks.Select(t => t.Id))
                    .ToArrayAsync();

                return await taskById.LoadAsync(taskIds, cancellationToken);
            }

            public async Task<IEnumerable<User>> GetMembersAsync(
                Family family,
                [ScopedService] ApplicationDbContext dbContext,
                UserByIdDataLoader userById,
                CancellationToken cancellationToken)
            {
                int[] userIds = await dbContext.Families
                    .Where(s => s.Id == family.Id)
                    .Include(s => s.Members)
                    .SelectMany(s => s.Members.Select(t => t.Id))
                    .ToArrayAsync();

                return await userById.LoadAsync(userIds, cancellationToken);
            }

        }
    }
}
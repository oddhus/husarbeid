using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using husarbeid.Data;
using husarbeid.DataLoader;

namespace husarbeid.Types
{
    public class FamilyTaskType : ObjectType<FamilyTask>
    {
        protected override void Configure(IObjectTypeDescriptor<FamilyTask> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) => ctx.DataLoader<FamilyTaskByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(t => t.Family)
                .ResolveWith<FamilyTaskResolver>(t => t.GetFamilyAsync(default!, default!, default));

            descriptor
                .Field(t => t.AssignedTo)
                .ResolveWith<FamilyTaskResolver>(t => t.GetUserAsync(default!, default!, default));

            descriptor
                .Field(t => t.FamilyId)
                .ID(nameof(Family));
        }


        private class FamilyTaskResolver
        {
            public async Task<Family?> GetFamilyAsync(
                FamilyTask familyTask,
                FamilyByIdDataLoader familyById,
                CancellationToken cancellationToken)
            {
                if (familyTask.FamilyId is null)
                {
                    return null;
                }

                return await familyById.LoadAsync(familyTask.FamilyId.Value, cancellationToken);
            }

            public async Task<User?> GetUserAsync(
                FamilyTask familyTask,
                UserByIdDataLoader userById,
                CancellationToken cancellationToken)
            {
                if (familyTask.AssignedToId is null)
                {
                    return null;
                }

                return await userById.LoadAsync(familyTask.AssignedToId.Value, cancellationToken);
            }
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using husarbeid.Data;
using Microsoft.EntityFrameworkCore;

namespace husarbeid.FamilyTasks
{
    [ExtendObjectType(name: "Mutation")]
    public class FamilyTaskMutations
    {

        [UseApplicationDbContext]
        public async Task<AddFamilyTaskPayload> AddFamilyTaskAsync(
            AddFamilyTaskInput input,
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            var familyTask = new FamilyTask
            {
                ShortDescription = input.ShortDescription,
                Payment = input.Payment,
                FamilyId = input.FamilyId,
                isCompleted = false
            };

            context.FamilyTasks.Add(familyTask);
            await context.SaveChangesAsync(cancellationToken);

            return new AddFamilyTaskPayload(familyTask);
        }

        // [UseApplicationDbContext]
        // public async Task<AddFamilyTaskPayload> RemoveFamilyTaskAsync(
        //     RemoveFamilyTasksInput input,
        //     [ScopedService] ApplicationDbContext context,
        //     CancellationToken cancellationToken)
        // {
        //     context.FamilyTasks.Re(familyTask);
        //     await context.SaveChangesAsync(cancellationToken);

        //     return new AddFamilyTaskPayload(familyTask);
        // }



    }


}
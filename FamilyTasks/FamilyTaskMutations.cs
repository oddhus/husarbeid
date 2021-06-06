using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using husarbeid.Common;
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
            [GlobalStateAttribute("currentUserId")] int? currentUserId,
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            if (currentUserId == null)
            {
                return new AddFamilyTaskPayload(
                    new UserError("Please sign in and try again", "NOT_AUTHORIZED"));
            }

            var familyTask = new FamilyTask
            {
                ShortDescription = input.ShortDescription,
                Payment = input.Payment,
                FamilyId = input.FamilyId,
                isCompleted = false,
                CreatedById = currentUserId,
                createdOn = DateTime.UtcNow,
                updatedOn = DateTime.UtcNow
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
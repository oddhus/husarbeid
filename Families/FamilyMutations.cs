using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using husarbeid.Data;
using husarbeid.Common;
using Microsoft.EntityFrameworkCore;

namespace husarbeid.Families
{
    [ExtendObjectType(name: "Mutation")]
    public class FamilyMutations
    {
        [UseApplicationDbContext]
        public async Task<AddFamilyPayload> CreateFamilyAsync(
            AddFamilyInput input,
            [GlobalStateAttribute("currentUserId")] int? currentUserId,
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            if (currentUserId == null)
            {
                return new AddFamilyPayload(
                    new UserError("Please sign in and try again", "NOT_AUTHORIZED"));
            }

            User user = await context.Users.FirstOrDefaultAsync(
                u => u.Id == currentUserId, cancellationToken);

            if (user == null)
            {
                return new AddFamilyPayload(
                    new UserError("No valid user was found", "USER_NOT_VALID"));
            }

            var family = new Family
            {
                FamilyName = input.FamilyName,
                Members = new List<User>(){
                    user
                }
            };

            context.Families.Add(family);
            await context.SaveChangesAsync(cancellationToken);

            return new AddFamilyPayload(family);
        }

        [UseApplicationDbContext]
        public async Task<AddFamilyPayload> AddFamilyMembersAsync(
            AddFamilyMembersInput input,
            [GlobalStateAttribute("currentUserId")] int? currentUserId,
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            if (currentUserId == null)
            {
                return new AddFamilyPayload(
                    new UserError("Please sign in and try again", "NOT_AUTHORIZED"));
            }

            User user = await context.Users.FirstOrDefaultAsync(
                u => u.Id == currentUserId, cancellationToken);

            Family family = await context.Families
                .FirstOrDefaultAsync(f => f.Id == input.FamilyId, cancellationToken);

            if (family is null || !family.Members.Contains(user))
            {
                return new AddFamilyPayload(
                    new UserError("You can only add members to your own families", "NOT_ALLOWED"));
            }

            User newMember = await context.Users
                .FirstOrDefaultAsync(u => u.Id == input.MemberId, cancellationToken);

            if (newMember is null)
            {
                return new AddFamilyPayload(
                    new UserError("User not found.", "USER_NOT_FOUND"));
            }

            family.Members.Add(newMember);
            await context.SaveChangesAsync(cancellationToken);

            return new AddFamilyPayload(family);
        }
    }


}
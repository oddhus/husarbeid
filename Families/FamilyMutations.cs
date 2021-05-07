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
        public async Task<AddFamilyPayload> AddFamilyAsync(
            AddFamilyInput input,
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            User user = await context.Users.FirstOrDefaultAsync(
                u => u.Id == input.CreatorId, cancellationToken);

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
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            Family family = await context.Families.FindAsync(input.FamilyId);

            if (family is null)
            {
                return new AddFamilyPayload(
                    new UserError("Family not found.", "FAMILY_NOT_FOUND"));
            }

            User newMember = await context.Users.FindAsync(input.MemberId);

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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using husarbeid.Common;
using husarbeid.Data;
using husarbeid.DataLoader;

namespace husarbeid.FamilyTasks
{
    public class AddFamilyTaskPayload : FamilyTaskPayloadBase
    {
        public AddFamilyTaskPayload(FamilyTask familyTask)
            : base(familyTask)
        {
        }

        public AddFamilyTaskPayload(UserError error)
            : base(new[] { error })
        {

        }

        public async Task<Family?> GetFamilyAsync(
            FamilyByIdDataLoader familyById,
            CancellationToken cancellationToken)
        {
            if (FamilyTask is null)
            {
                return null;
            }

            return await familyById.LoadAsync(FamilyTask.Id, cancellationToken);
        }

    }
}
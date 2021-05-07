using System.Collections.Generic;
using husarbeid.Common;
using husarbeid.Data;

namespace husarbeid.Families
{
    public class AddFamilyPayload : FamilyPayloadBase
    {
        public AddFamilyPayload(Family family)
            : base(family)
        {
        }

        public AddFamilyPayload(UserError error)
            : base(new[] { error })
        {
        }
    }
}
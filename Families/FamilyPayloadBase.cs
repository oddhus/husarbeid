using System.Collections.Generic;
using husarbeid.Common;
using husarbeid.Data;

namespace husarbeid.Families
{
    public class FamilyPayloadBase : Payload
    {
        protected FamilyPayloadBase(Family family)
        {
            Family = family;
        }

        protected FamilyPayloadBase(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public Family? Family { get; }
    }
}
using System.Collections.Generic;
using husarbeid.Common;
using husarbeid.Data;

namespace husarbeid.FamilyTasks
{
    public class FamilyTaskPayloadBase : Payload
    {
        protected FamilyTaskPayloadBase(FamilyTask familyTask)
        {
            FamilyTask = familyTask;
        }

        protected FamilyTaskPayloadBase(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public FamilyTask? FamilyTask { get; }
    }
}
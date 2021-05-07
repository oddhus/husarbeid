using System;
using HotChocolate.Types.Relay;
using husarbeid.Data;

namespace husarbeid.FamilyTasks
{
    public record AddFamilyTaskInput(
        string ShortDescription,
        int Payment,
        [ID(nameof(Family))] int FamilyId);
}
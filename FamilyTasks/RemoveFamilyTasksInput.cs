using System;
using System.Collections.Generic;
using HotChocolate.Types.Relay;
using husarbeid.Data;

namespace husarbeid.FamilyTasks
{
    public record RemoveFamilyTasksInput(
        [ID(nameof(FamilyTask))] IReadOnlyList<int> TaskIds);
}
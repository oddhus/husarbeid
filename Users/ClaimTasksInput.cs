using System;
using System.Collections.Generic;
using HotChocolate.Types.Relay;
using husarbeid.Data;

namespace husarbeid.Users
{
    public record ClaimTaskInput(
        [ID(nameof(User))] int UserId,
        [ID(nameof(FamilyTask))] IReadOnlyList<int> TaskIds);
}
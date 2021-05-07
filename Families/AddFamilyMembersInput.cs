using System;
using System.Collections.Generic;
using HotChocolate.Types.Relay;
using husarbeid.Data;

namespace husarbeid.Families
{
    public record AddFamilyMembersInput(
        [ID(nameof(Family))] int FamilyId,
        [ID(nameof(User))] int MemberId);
}